using UnityEngine;

namespace Paerux.Persistence
{
    public class RandomData : MonoBehaviour, IBindable
    {
        public void Bind(ISaveData data)
        {
            Debug.Log("Binding " + name);
        }
    }
}