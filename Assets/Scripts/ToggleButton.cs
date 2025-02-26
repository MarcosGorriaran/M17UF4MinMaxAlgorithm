using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
[RequireComponent (typeof(Image))]
public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    private Sprite _toggledOnImage;
    [SerializeField] 
    private Sprite _toggledOffImage;
    [SerializeField]
    private AudioSource _toggleOnSound;
    [SerializeField]
    private AudioSource _toggleOffSound;
    
    private Button _toggleButton;
    private Image _buttonImage;
    [SerializeField]
    private bool _toggledOn;
    void Start()
    {
        _toggleButton = GetComponent<Button>();
        _toggleButton.onClick.AddListener(Toggle);
        _buttonImage = GetComponent<Image>();
        _buttonImage.sprite = _toggledOn ? _toggledOnImage : _toggledOffImage;
    }
    void Toggle()
    {
        _toggledOn = !_toggledOn;

        _buttonImage.sprite = _toggledOn ? _toggledOnImage : _toggledOffImage;
        if( _toggledOn)
        {
            _buttonImage.sprite = _toggledOnImage;
            if(_toggleOnSound != null)
            {
                _toggleOnSound.Play();
                _toggleOffSound.Stop();
            }
        }
        else
        {
            _buttonImage.sprite = _toggledOffImage;
            if(_toggleOffSound != null)
            {
                _toggleOffSound.Play();
                _toggleOnSound.Stop();
            }
            
        }
    }
    public bool IsOn()
    {
        return _toggledOn;
    }

    
}
