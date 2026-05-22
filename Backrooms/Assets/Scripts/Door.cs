using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen = false;

    void Start()
    {
        _animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
        
        if (_animator == null)
        {
            Debug.LogError($"❌ Ошибка на объекте {gameObject.name}: Компонент Animator не найден ни на самом объекте, ни в его детях!");
        }
    }

    public void OpenDoor()
    {
        if (!_isOpen)
        {
            Debug.Log("🚪 Скрипт Door поймал сигнал! Активируем триггер 'Open'...");
            
            if (_animator != null)
            {
                _animator.SetTrigger("Open"); 
            }
            
            _isOpen = true;
            
            Collider doorCollider = GetComponent<Collider>() ?? GetComponentInChildren<Collider>();
            if (doorCollider != null)
            {
                doorCollider.isTrigger = true; 
            }
        }
    }
}