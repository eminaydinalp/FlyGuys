using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMove
{
    void Move(PathCreation.VertexPath path = null, float speed = 5, float xUnit = 0f);
}

public interface IPlayerSlide
{
    void Slide(PathCreation.VertexPath path = null);
}
