using UnityEngine;

public interface PlayerAction
{
    void buildWall(Wallmaterial wm);
    void prassScreen();//this function will notice whose cnnonball
    void unwind();//unwind will made tank can't move,but more powerful
}
public enum Wallmaterial
{
    metal,normal,water
}
public enum KeyType
{
    Key1, Key2
}
public class ButtonAction : MonoBehaviour, PlayerAction
{
    void PlayerAction.buildWall(Wallmaterial wm)
    {
        throw new System.NotImplementedException();
    }
    public void PressKey(GameObject _KeyObj)
    {
       
    }

    void PlayerAction.prassScreen()
    {
        Input.GetMouseButtonUp((int)KeyCode.Mouse0);
    }

    void PlayerAction.unwind()
    {
        throw new System.NotImplementedException();
    }
}