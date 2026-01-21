using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // 移動先シーン名
    public int touchingEnemyCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            touchingEnemyCount++;
            Debug.Log("Enemy entered! Current count: " + touchingEnemyCount);

            // もし同時に3体以上接触したらシーン移行
            if (touchingEnemyCount >= 3)
            {
                LoadNextScene();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            touchingEnemyCount = Mathf.Max(0, touchingEnemyCount - 1);
            Debug.Log("Enemy exited! Current count: " + touchingEnemyCount);
        }
    }
    // UI Button の OnClick から呼ぶ
    public void OnClickChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
