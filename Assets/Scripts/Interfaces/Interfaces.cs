using UnityEngine;
public interface IState
{
    void EnterState();
    void ExitState();
}

public interface IBindInput
{
    void BindInput();
    void UnbindInput();
}

public interface IUpdateLoop
{
    void Update();
    void FixedUpdate();
}

public interface ICollision2D
{
    void OnCollisionEnter2D(Collision2D collision);
    void OnCollisionStay2D(Collision2D collision);
    void OnCollisionExit2D(Collision2D collision);
}

public interface IDamageable
{
    bool TakeDamage(float damage, Vector2 direction);
}