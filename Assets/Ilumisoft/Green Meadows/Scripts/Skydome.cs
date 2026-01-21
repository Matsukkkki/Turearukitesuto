using UnityEngine;

namespace Ilumisoft.GreenMeadows
{
    public class Skydome : MonoBehaviour
    {
        private Transform mainCameraTransform;

        private void Awake()
        {
            // 最初に MainCamera を探してキャッシュ
            if (Camera.main != null)
            {
                mainCameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Skydome: MainCamera が見つかりません。スカイドームは追従しません。");
            }
        }

        private void LateUpdate()
        {
            // カメラが存在しない場合は処理しない
            if (mainCameraTransform == null)
                return;

            // カメラの位置に合わせてスカイドームを移動
            Vector3 position = mainCameraTransform.position;

            // Y軸は固定（プレイヤーがジャンプしても変わらない）
            position.y = transform.position.y;

            transform.position = position;
        }
    }
}
