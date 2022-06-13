using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameLoopListener
{
    void Listen(GameLoop.GameStage stage);
}
