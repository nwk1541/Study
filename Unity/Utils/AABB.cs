using UnityEngine;

// 2D Collision detection : Axis-Aligned Bounding Box
// ref : https://developer.mozilla.org/en-US/docs/Games/Techniques/2D_collision_detection

public static class AABB
{
    public static bool IsCollision(BoxCollider2D left, BoxCollider2D right)
    {
        Rect leftBox = left.BoxToRect();
        Rect rightBox = right.BoxToRect();

        return Intersect(leftBox, rightBox);
    }

    public static bool Intersect(Rect left, Rect right)
    {
        bool cond1 = left.yMin > right.yMax;
        bool cond2 = left.yMax < right.yMin;
        bool cond3 = left.xMin < right.xMax;
        bool cond4 = left.xMax > right.xMin;

        return cond1 && cond2 && cond3 && cond4;
    }

    private static Rect BoxToRect(this BoxCollider2D collider)
    {
        Vector2 pos = collider.transform.position;

        pos += collider.offset;

        pos.x -= collider.size.x / 2;
        pos.y += collider.size.y / 2;

        return new Rect(pos.x, pos.y, collider.size.x, -collider.size.y);
    }
}