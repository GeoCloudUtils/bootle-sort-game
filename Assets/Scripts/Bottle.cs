using DG.Tweening;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Task = System.Threading.Tasks.Task;

public class Bottle : MonoBehaviour
{
    [SerializeField] private Button bottleButton;
    [SerializeField] private Image bottleImage;

    public event UnityAction<Bottle> BottleClicked;

    private bool _isSelected;

    public void Initialize(Color color)
    {
        bottleImage.color = color;
        bottleButton.onClick.AddListener(OnBottleClick);
    }

    private void OnBottleClick()
    {
        BottleClicked?.Invoke(this);
        if (_isSelected) return;
        OnSelect();
    }

    public Color GetColor()
    {
        return bottleImage.color;
    }

    private void OnSelect()
    {
        _isSelected = true;
        bottleImage.transform.DOScale(1.1f, 0.5f).SetEase(Ease.OutBack);
    }

    public async void OnBottleDeslect()
    {
        bottleImage.transform.DOKill();
        bottleImage.transform.localScale = Vector3.one;
        await Task.Delay(100);
        _isSelected = false;
    }
}