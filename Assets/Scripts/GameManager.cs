using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    private AudioSource audio;
    public AudioClip[] audioClip;
    private BallController player;

    private GroundPiece[] allGroundPiece;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        SetupNewLevel();
        player = FindObjectOfType<BallController>();
    }
    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
        else if(singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinish;
    }
    private void OnLevelFinish(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetupNewLevel()
    {
        allGroundPiece = FindObjectsOfType<GroundPiece>();
    }
    public void CheckComplete()
    {
        bool isFinished = true;
        for(int i = 0; i < allGroundPiece.Length; i++)
        {
            if(allGroundPiece[i].isColored ==false)
            {
                isFinished = false;
                break;
            }
        }
        if(isFinished)
        {
            NextLevel();
        }
    }
    private void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex ==5)
        {
            SceneManager.LoadScene(0);
        }
        StartCoroutine(LoadSceneMenu(5));
        
        
    }
    public void PlaySound(string name)
    {
        switch (name)
        {
            case "roll":
                audio.PlayOneShot(audioClip[0], 0.8f);
                break;
            case "levelFinish":
                audio.PlayOneShot(audioClip[1], 1);
                break;
        }
    }
    IEnumerator LoadSceneMenu(float duration)
    {
        player.GameFinish();
        player.isTravelling = true;
        PlaySound("levelFinish");
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
