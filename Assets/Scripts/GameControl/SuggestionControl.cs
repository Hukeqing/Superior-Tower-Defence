using UnityEngine;
using UnityEngine.UI;

public class SuggestionControl : MonoBehaviour
{
    public Text text;

    public Toggle T_MuteSiri;

    public Dropdown PerDropdowm;
    public Slider spdSlider;
    public Slider pitSlider;
    public Slider volSlider;

    public Text Pingtime;

    public InputField TestIF;

    private Animator animator;
    private bool UsingSiri;
    private TtsService TS;
    private AudioSource AS;

    private bool NetWork;

    private PerStatus per;
    private int spd;
    private int pit;
    private int vol;

    private void Start()
    {
        animator = GetComponent<Animator>();
        TS = GetComponent<TtsService>();
        AS = GetComponent<AudioSource>();
        UsingSiri = false;
        NetJudge();

        per = PerStatus.Man; PerDropdowm.value = 0;
        spd = 5; spdSlider.value = 5;
        pit = 5; pitSlider.value = 5;
        vol = 5; volSlider.value = 5;
    }

    public void NetJudge()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            T_MuteSiri.interactable = false;
            TestIF.interactable = false;
            NetWork = false;
            Pingtime.text = "检查网络";
        }
        else
        {
            T_MuteSiri.interactable = true;
            TestIF.interactable = true;
            NetWork = true;
            StartCoroutine(TS.StartConnecting(re =>
            {
                Pingtime.text = re ? "连接成功" : "连接超时";
            }));
        }
    }

    public void MuteSiri() => UsingSiri = !T_MuteSiri.isOn;
    public void Suggest(string s)
    {
        animator.SetTrigger("Stop");
        text.text = s;
        animator.SetTrigger("Suggest");
        SiriSound(s);
    }
    public void ChangePer()
    {
        per = (PerStatus)PerDropdowm.value;
        if ((int)per >= 2) per++;
    }
    public void ChangeSpd() => spd = (int)spdSlider.value;
    public void Changepit() => pit = (int)pitSlider.value;
    public void Changevol() => vol = (int)volSlider.value;

    public void SiriSound(string s)
    {
        if (UsingSiri && NetWork)
        {
            PlayClip(s);
        }
    }
    public void SiriTest()
    {
        if (!NetWork)
        {
            NetJudge();
        }
        else
        {
            if (string.IsNullOrWhiteSpace(TestIF.text))
            {
                TestIF.text = "这是一条语音测试";
            }
            PlayClip(TestIF.text);
        }
    }
    private void PlayClip(string s)
    {
        if (NetWork)
        {
            StartCoroutine(TS.GetAudio(s, per, spd, pit, vol, re =>
            {
                if (re.Success)
                {
                    AS.Stop();
                    AS.clip = re.clip;
                    AS.Play();
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log(re.err_msg);
#endif
                }
            }));
        }
    }
}