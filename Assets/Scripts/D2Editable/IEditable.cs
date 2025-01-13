using UnityEngine;
using UnityEngine.SceneManagement;
namespace Diablo2Editor
{
    public interface IEditable
    {
        public void OnSceneLoaded(GameObject gameObject);
    }
}
