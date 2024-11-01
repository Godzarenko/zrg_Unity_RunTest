using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundsManager : MonoBehaviour
{
    public List<UnityEngine.Audio.AudioMixerGroup> Mixers;
    public List<SoundBankGroup> Groups = new List<SoundBankGroup>();
    //private static Camera Camera
    //{
    //    get
    //    {
    //        if (!_camera || !_camera.gameObject.activeInHierarchy)
    //            _camera = Camera.main;
    //        return _camera;
    //    }
    //}

    //private static Camera _camera;

    private static AudioListener Listener
    {
        get
        {
            if (_lstn == null || !_lstn.isActiveAndEnabled)
            {
                _lstn = FindObjectOfType<AudioListener>();
            }
            return _lstn;
        }
    }
    static AudioListener _lstn;

    public static GameObject WorldEmitterPrefab
    {
        get
        {
            return Instance.worldEmitterPrefab;
        }
    }
    public static GameObject UiEmitterPrefab
    {
        get
        {
            return Instance.uiEmitterPrefab;
        }
    }

    public static List<SoundEmitter> Sounds = new List<SoundEmitter>();

    public static SoundsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundsManager>();
            }
            return _instance;
        }
    }
    public static SoundsManager _instance;

    public static GameObject _worldEmitterPrefab
    {
        get
        {
            return Instance.worldEmitterPrefab;
        }
    }

    public GameObject worldEmitterPrefab;
    public GameObject uiEmitterPrefab;

    [SerializeField] private float globalMusicModifier = .2f;
    [Range(0f, 10f)]
    public float SoundVol = 1;
    [Range(-80f, 20f)]
    public float SoundMaxGain = 7f;
    float musicCD;
    int lastMus = -1;
    int currMus = -1;

    public static GameObject LastCamera;


    // Start is called before the first frame update
    void Start()
    {
        Sounds = new List<SoundEmitter>();
    }

    public void ChangeMusicVol(float obj)
    {
    }

    // Update is called once per frame
    void Update()
    {
        List<SoundEmitter> Killed = new List<SoundEmitter>();
        for (int i = 0; i < Sounds.Count; i++)
        {

            SoundEmitter SE = Sounds[i];
            if (SE.GO == null)
            {
                Killed.Add(SE);
                continue;
            }
            if (SE.lifespan > 0)
            {
                SE.lifespan -= Time.deltaTime;
                if (SE.lifespan <= 0)
                {
                    Destroy(SE.GO);
                    Killed.Add(SE);
                }
            }
            if (SE.GO != null)
            {
                if (SE.KillOnSoundEnd)
                {
                    if (!SE.AS.isPlaying)
                    {
                        Destroy(SE.GO);
                        Killed.Add(SE);
                    }
                    else
                    {
                        if (SE.AS.time >= (SE.AC.length * SE.AS.pitch))
                        {
                            Destroy(SE.GO);
                            Killed.Add(SE);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < Killed.Count; i++)
        {
            Sounds.Remove(Killed[i]);
        }
    }

    public static SoundClip RandomClipFromList(List<SoundClip> Arr)
    {
        if (Arr.Count == 0)
        {
            return null;
        }
        return Arr[Random.Range(0, Arr.Count)];
    }

    public static SoundClip RandomClipFromBank(string bak)
    {
        if (Instance == null)
        {
            return null;
        }

        string[] Names = bak.Split(':');

        if (Names.Length < 2)
        {
            return null;
        }
        if (Names[0].Length <= 1 || Names[1].Length <= 1)
        {
            return null;
        }

        for (int i = 0; i < Instance.Groups.Count; i++)
        {
            SoundBankGroup SBG = Instance.Groups[i];
            if (SBG.Name == Names[0])
            {
                for (int j = 0; j < SBG.Banks.Count; j++)
                {
                    if (SBG.Banks[j].Name == Names[1])
                    {
                        return RandomClipFromList(SBG.Banks[j].Clips);
                    }
                }
                return null;
            }
        }
        return null;
    }

    public static SoundEmitter CreateSoundOnPos(string Name, float Lifespan, bool KillOnSoundEnd, Vector3 Pos)
    {
        return CreateSpatialSoundOnPos(RandomClipFromBank(Name), Lifespan, KillOnSoundEnd, Pos);
    }
    public static SoundEmitter CreateSoundOnObject(string Name, float Lifespan, bool KillOnSoundEnd, GameObject Obj)
    {
        return CreateSpatialSoundOnObject(RandomClipFromBank(Name), Lifespan, KillOnSoundEnd, Obj, WorldEmitterPrefab);
    }
    public static SoundEmitter CreateSoundOnCamera(string Name, float Lifespan, bool KillOnEnd)
    {
        // Debug.Log($"{Name} - {nameof(CreateSoundOnCamera)} invoked from {new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().ReflectedType.Name}::{new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name}");

        SoundClip AC = RandomClipFromBank(Name);
        if (AC == null)
        {
            return null;
        }
        if (LastCamera == null || !LastCamera.gameObject.activeInHierarchy)
        {
            //LastCamera = Camera.gameObject;
            LastCamera = Listener.gameObject;// GameObject.FindObjectOfType<AudioListener>().gameObject;//Camera.gameObject;
        }
        return CreateFlatSoundOnObject(AC, Lifespan, KillOnEnd, LastCamera, UiEmitterPrefab);
    }
    public static SoundEmitter CreateSoundOnCamera(SoundClip AC, float Lifespan, bool KillOnEnd)
    {
        if (AC == null)
        {
            return null;
        }
        if (LastCamera == null || !LastCamera.gameObject.activeInHierarchy)
        {
            LastCamera = Listener.gameObject;// GameObject.FindObjectOfType<AudioListener>().gameObject;//Camera.gameObject;
        }
        return CreateFlatSoundOnObject(AC, Lifespan, KillOnEnd, LastCamera, UiEmitterPrefab);
    }
    public static SoundEmitter CreateSpatialSoundOnPos(SoundClip AC, float Lifespan, bool KillOnSoundEnd, Vector3 Pos)
    {
        if (AC == null)
        {
            return null;
        }
        SoundEmitter SE = new SoundEmitter();

        SE.lifespan = Lifespan;
        SE.AC = AC.Clip;
        SE.KillOnSoundEnd = KillOnSoundEnd;

        GameObject NewGO = Instantiate(WorldEmitterPrefab);
        NewGO.transform.position = Pos;
        AudioSource AS = NewGO.GetComponent<AudioSource>();

        AS.clip = AC.Clip;
        AS.volume = AC.ClipVolume * Instance.SoundVol;

        AS.pitch = AC.BasePitch + Random.Range(0f, AC.PitchRandom);

        AS.Play();

        AS.outputAudioMixerGroup = Instance.Mixers[0];

        SE.AS = AS;

        SE.GO = NewGO;

        Sounds.Add(SE);

        return SE;
    }
    public static SoundEmitter CreateSpatialSoundOnObject(SoundClip AC, float Lifespan, bool KillOnSoundEnd, GameObject Obj, GameObject EmitterPrefab)
    {
        if (AC == null)
        {
            return null;
        }
        if (Obj == null)
        {
            return null;
        }
        SoundEmitter SE = new SoundEmitter();

        SE.lifespan = Lifespan;
        SE.AC = AC.Clip;
        SE.KillOnSoundEnd = KillOnSoundEnd;

        GameObject NewGO = Instantiate(EmitterPrefab);
        NewGO.transform.position = Obj.transform.position;
        NewGO.transform.parent = Obj.transform;
        AudioSource AS = NewGO.GetComponent<AudioSource>();

        AS.clip = AC.Clip;
        AS.volume = AC.ClipVolume * Instance.SoundVol;

        AS.pitch = AC.BasePitch + Random.Range(0f, AC.PitchRandom);

        AS.Play();

        AS.outputAudioMixerGroup = Instance.Mixers[0];

        SE.AS = AS;

        SE.GO = NewGO;

        Sounds.Add(SE);

        return SE;
    }
    public static SoundEmitter CreateFlatSoundOnObject(SoundClip AC, float Lifespan, bool KillOnSoundEnd, GameObject Obj, GameObject EmitterPrefab)
    {
        if (AC == null)
        {
            return null;
        }
        if (Obj == null)
        {
            return null;
        }
        SoundEmitter SE = new SoundEmitter();

        SE.lifespan = Lifespan;
        SE.AC = AC.Clip;
        SE.KillOnSoundEnd = KillOnSoundEnd;

        GameObject NewGO = Instantiate(EmitterPrefab);
        NewGO.transform.position = Obj.transform.position;
        NewGO.transform.parent = Obj.transform;
        AudioSource AS = NewGO.GetComponent<AudioSource>();

        AS.clip = AC.Clip;
        AS.volume = AC.ClipVolume * Instance.SoundVol;

        AS.pitch = AC.BasePitch + Random.Range(0f, AC.PitchRandom);

        AS.Play();

        AS.outputAudioMixerGroup = Instance.Mixers[0];

        SE.AS = AS;
        AS.spatialBlend = 0f;

        SE.GO = NewGO;

        Sounds.Add(SE);

        return SE;
    }
    [System.Serializable]
    public class SoundEmitter
    {
        public AudioClip AC;
        public AudioSource AS;
        public float lifespan = -1;
        public bool KillOnSoundEnd;
        public GameObject GO;
    }
    [System.Serializable]
    public class SoundBankGroup
    {
        public string Name;
        public List<SoundBank> Banks;
    }
    [System.Serializable]
    public class SoundBank
    {
        public string Name;
        public List<SoundClip> Clips;
    }
    [System.Serializable]
    public class SoundClip
    {
        public AudioClip Clip;
        [Range(0, 2)]
        public float ClipVolume = 1;
        [Range(-3, 3)]
        public float BasePitch = 1;
        [Range(0, 3)]
        public float PitchRandom = 0;
    }
}
