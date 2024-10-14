using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
   
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points = 0;
    private int highScore;
    
    private bool m_GameOver = false;
public Text Name;
public Text GameOverScoreText;
public Text GameOverNameText;
private string highScoreName;
public Text HighScoreText;
public Text HighScoreNameText;

   
    

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
                PlayerName();
                LoadScore();
                HighScoreText.text = $"High Score: {highScore}";
                HighScoreNameText.text = $"High Score Player: {highScoreName}";
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        if (!m_GameOver)
        {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        CheckHighScore();
        }
    }
    private void CheckHighScore()
    {
        if (m_Points> highScore)
        {
            highScore = m_Points;
            highScoreName = Menu.playerName;
        }
    }
    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveScore();
        DisplayGameOverInfo();
        
    }
    private void DisplayGameOverInfo()
    {
        GameOverScoreText.text = $"FinalScore: {m_Points}";
        GameOverNameText.text = $"Player: {Menu.playerName}";
    }
    public void PlayerName()
    {
        Name.text = Menu.playerName;
    }
    [System.Serializable]
    
        class SaveData
    {
        public int HighScore;
        public string PlayerName;
    }
    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.HighScore = highScore;
        data.PlayerName = highScoreName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/saveFile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.HighScore;
            highScoreName = data.PlayerName;
        }
    }
}
