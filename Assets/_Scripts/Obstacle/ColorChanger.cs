using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float _lerpTime;
    [SerializeField] private Color _mainColor;
    [SerializeField] private Color _subColor;
    [SerializeField] private string _mainColorPropName;
    [SerializeField] private string _subColorPropName;

    private Material _material;
    private Color _initialMainColor;
    private Color _initialSubColor;

    public async Task ChangeColor(Obstacle obstacle)
    {
        _material = obstacle.Material;

        Color MainColorShader() => _material.GetColor(_mainColorPropName);
        Color SubColorShader() => _material.GetColor(_subColorPropName);
        Color setMainColor;
        Color setSubColor;

        //Colors are no changed yet
        _initialMainColor = MainColorShader();
        _initialSubColor = SubColorShader();

        while (MainColorShader() != _mainColor)
        {
            setMainColor = Color.Lerp(MainColorShader(), _mainColor, _lerpTime);
            setSubColor = Color.Lerp(SubColorShader(), _subColor, _lerpTime);

            _material.SetColor(_mainColorPropName, setMainColor);
            _material.SetColor(_subColorPropName, setSubColor);

            await Task.Delay(10);
        }
    }

    public void ReturnInitialColors()
    {
        _material.SetColor(_mainColorPropName, _initialMainColor);
        _material.SetColor(_subColorPropName, _initialSubColor);
    }
}
