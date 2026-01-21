using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // 移動先シーン名
    [Header("HP")]
    public int maxHP = 100;
    private float currentHP; // 内部計算は float
    public int damagePerSecondPerEnemy = 30;
    public int regenPerSecond = 15;

    [Header("Ambient Light")]
    public Color ambientNormal = Color.white;
    public Color ambientLowHP = new Color(0.4f, 0.1f, 0.1f); // 赤暗い
    public float ambientIntensityNormal = 1f;
    public float ambientIntensityLowHP = 0.2f;

    [Header("Light")]
    public Light directionalLight;
    public float maxKelvin = 6000f;
    public float minKelvin = 1000f;
    public float maxLightIntensity = 1f;
    public float minLightIntensity = 0.2f;

    [Header("UI")]
    public GameObject gameUI;
    public GameObject gameOverUI;
    public Text hpText; // HP表示用Text

    int touchingEnemyCount = 0;
    bool isDead = false;

    void Start()
    {
        currentHP = maxHP;

        directionalLight.useColorTemperature = true;
        UpdateLightByHP();
        UpdateHPUI();
    }

    void Update()
    {
        if (!isDead)
        {
            float delta = Time.deltaTime;

            // ---------- HP処理 ----------
            if (touchingEnemyCount > 0)
            {
                currentHP -= damagePerSecondPerEnemy * touchingEnemyCount * delta;
            }
            else
            {
                currentHP += regenPerSecond * delta;
            }

            // Clamp & 小数点以下は無視（表示用に丸めるだけ）
            currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

            if (currentHP <= 0f && !isDead)
            {
                currentHP = 0f;
                isDead = true;
                StartCoroutine(GameOverSequence());
            }
        }

        // ---------- ライト演出 ----------
        UpdateLightByHP();

        // ---------- HP UI 更新 ----------
        UpdateHPUI();
    }

    void UpdateLightByHP()
    {
        float t = Mathf.InverseLerp(0f, 50f, currentHP);

        directionalLight.colorTemperature = Mathf.Lerp(minKelvin, maxKelvin, t);
        directionalLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, t);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Color.Lerp(ambientLowHP, ambientNormal, t);
        RenderSettings.ambientIntensity = Mathf.Lerp(ambientIntensityLowHP, ambientIntensityNormal, t);
    }

    void UpdateHPUI()
    {
        if (hpText != null)
        {
            // 小数点以下なし +「HP：」表記
            hpText.text = "HP：" + Mathf.FloorToInt(currentHP).ToString();
        }
    }


    IEnumerator GameOverSequence()
    {
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);

        // 1フレーム待つ（チラつき防止）
        yield return null;

        Debug.Log("GAME OVER UI SHOWN");
        // 3秒待つ
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(nextSceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            touchingEnemyCount++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            touchingEnemyCount = Mathf.Max(0, touchingEnemyCount - 1);
    }
}
