using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerController : MonoBehaviour
{
    static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<PlayerController>();
            }
            return _instance;
        }
    }

    [SerializeField] SplineContainer MainPathSpline;
    [SerializeField] float LinearSpeed;
    float pathLength;
    float splineSpeed;
    float splinePos;
    [SerializeField] Animator PlayerCaretAnimator;
    public Transform PlayerCaret;
    [SerializeField] Vector2 PlayerCaretXBounds;
    float pathWidth;
    bool prevClick;
    Vector3 prevClickPos;
    public float CurrentXP
    {
        get; private set;
    }
    [SerializeField] float StartXP;
    public int CurrentLevel
    {
        get; private set;
    }
    float _totalXPToMax;
    public float XPPercent
    {
        get
        {
            return _totalXPToMax != 0 ? CurrentXP / _totalXPToMax : 0;
        }
    }
    [SerializeField] PlayerLevelStruct[] PlayerLevels;

    bool LevelPlaying;

    [SerializeField] bool Debug_Editor_StickToSpline;
    private void OnDrawGizmos()
    {
        if (Debug_Editor_StickToSpline && !LevelPlaying)
        {
            Vector3 Tangent = transform.forward;
            Vector3 Pos = transform.position;
            Vector3 Up = Vector3.up;

            float3 fTangent;
            float3 fPos;
            float3 fUp;
            if (MainPathSpline.Evaluate(0, out fPos, out fTangent, out fUp)) //why it uses float3
            {
                Tangent = fTangent;
                Pos = fPos;

                transform.SetPositionAndRotation(
                    Pos,
                    Quaternion.LookRotation(Tangent, Up));
            }
        }
    }

    private void OnValidate()
    {
        if (PlayerLevels != null && PlayerLevels.Length > 0)
        {
            PlayerLevels[0].TotalXP = 0; //make sure first level need 0 xp to reach
        }
    }
    private void Awake()
    {
        LevelManager.Instance.OnGameContinue.AddListener(GameContinue);
    }
    private void Start()
    {
        Debug_Editor_StickToSpline = false;
        pathWidth = (PlayerCaretXBounds.y - PlayerCaretXBounds.x);
        CurrentLevel = -1;
        _totalXPToMax = 0;
        for (int i = 0; i < PlayerLevels.Length; i++)
        {
            _totalXPToMax = PlayerLevels[i].TotalXP;
            if(StartXP >= PlayerLevels[i].TotalXP)
            {
                CurrentLevel++;
            }
        }
        pathLength = MainPathSpline.CalculateLength();
        splineSpeed = LinearSpeed / pathLength;
        LevelManager.Instance.PlayerLevelChanged(CurrentLevel);
        CurrentXP = StartXP;
        LevelManager.Instance.PlayerXPChanged( StartXP / _totalXPToMax);

    }
    float _currentMoveMotion;
    private void Update()
    {
        if (LevelPlaying)
        {
            splinePos += splineSpeed * Time.deltaTime;
            Vector3 Tangent = transform.forward;
            Vector3 Pos = transform.position;
            Vector3 Up = Vector3.up;

            float3 fTangent;
            float3 fPos;
            float3 fUp;
            if (MainPathSpline.Evaluate(splinePos, out fPos, out fTangent, out fUp)) //why it uses float3
            {
                Tangent = fTangent;
                Pos = fPos;

                transform.SetPositionAndRotation(
                    Pos, 
                    Quaternion.LookRotation(Tangent, Up));
            }
            else
            {
                //end of a line
            }

            if (Input.GetMouseButton(0))
            {
                if (!prevClick)
                {
                    prevClick = true;
                    prevClickPos = Input.mousePosition;
                }
                Vector3 Delta = Input.mousePosition - prevClickPos;
                if (Delta.sqrMagnitude > 0)
                {
                    float X = Delta.x / Screen.width;

                    float AddX = pathWidth * X;

                    Vector3 LocalPlayerPos = PlayerCaret.localPosition;
                    LocalPlayerPos.x += AddX;
                    LocalPlayerPos.x = Mathf.Clamp(LocalPlayerPos.x, PlayerCaretXBounds.x, PlayerCaretXBounds.y);
                    PlayerCaret.localPosition = LocalPlayerPos;
                    prevClickPos = Input.mousePosition;
                    _currentMoveMotion = Mathf.MoveTowards(_currentMoveMotion, Mathf.Sign(X), 25 * Time.deltaTime);
                    
                }
                else
                {
                    _currentMoveMotion = Mathf.MoveTowards(_currentMoveMotion, 0, 10 * Time.deltaTime);
                }

            }
            else
            {
                _currentMoveMotion = Mathf.MoveTowards(_currentMoveMotion, 0, 15 * Time.deltaTime);
            }
            PlayerCaretAnimator.SetFloat("side", _currentMoveMotion);
        }
        if (failImune > 0)
        {
            failImune -= Time.deltaTime;
        }
    }

    public void AddXP(float xp)
    {
        int prevLevel = CurrentLevel;
        if (xp > 0)
        {
            if (CurrentLevel == PlayerLevels.Length - 1)
            {
                //last level, no XP GAIN
                CurrentXP = PlayerLevels[CurrentLevel].TotalXP;
            }
            else
            {
                CurrentXP += xp;
                while (CurrentXP > PlayerLevels[CurrentLevel + 1].TotalXP)
                {
                    CurrentLevel++;
                    if (CurrentLevel == PlayerLevels.Length - 1)
                    {
                        //last level, no XP GAIN
                        CurrentXP = PlayerLevels[CurrentLevel].TotalXP;
                        break;
                    }
                }
                LevelManager.Instance.PlayerXPChanged(XPPercent);
            }
        }
        else
        {
            if (failImune > 0)
            {
                return;
            }
            CurrentXP += xp;
            if (CurrentXP < 0)
            {
                PlayerFailed();
            }
            else
            {
                while (CurrentXP < PlayerLevels[CurrentLevel].TotalXP)
                {
                    CurrentLevel--;
                    if (CurrentLevel < 0)
                    {
                        //-1 level, first level missconfigured?
                        PlayerFailed();
                        break;
                    }
                }
            }
            LevelManager.Instance.PlayerXPChanged(XPPercent);
        }
        if (CurrentLevel != prevLevel)
        {
            PlayerLevelChanged();
        }
    }
    void PlayerLevelChanged()
    {
        LevelManager.Instance.PlayerLevelChanged(CurrentLevel);
    }
    void PlayerFailed()
    {
        LevelPlaying = false;
        LevelManager.Instance.SignalGameEnd(false, 0);
        PlayerCaretAnimator.SetBool("fail", true);
    }
    float failImune = 0;
    public void GameContinue()
    {
        CurrentLevel = -1;
        for (int i = 0; i < PlayerLevels.Length; i++)
        {
            if (StartXP >= PlayerLevels[i].TotalXP)
            {
                CurrentLevel++;
            }
        }
        CurrentXP = StartXP;
        LevelManager.Instance.PlayerLevelChanged(CurrentLevel);
        LevelManager.Instance.PlayerXPChanged(StartXP / _totalXPToMax);
        LevelPlaying = true;
        PlayerCaretAnimator.SetBool("play", true);
        PlayerCaretAnimator.SetBool("fail", false);
        failImune = 3f;
    }
    public void StartGame()
    {
        LevelPlaying = true;
        PlayerCaretAnimator.SetBool("play", true);
    }
}
[System.Serializable]
public struct PlayerLevelStruct
{
    public float TotalXP; //what total XP needed to achieve this level
}