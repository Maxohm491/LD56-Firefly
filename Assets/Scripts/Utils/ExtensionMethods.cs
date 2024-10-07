using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Firefly.Utils
{
    public static class ExtensionMethods
    {
        public static bool InBound(this Vector3 v, Vector3 min, Vector3 max)
        {
            return min.x <= v.x && v.x <= max.x &&
                min.y <= v.y && v.y <= max.y;
        }

        public static float Dot(this Vector3 v, Vector3 u)
        {
            return v.x * u.x + v.y * u.y + v.z * u.z;
        }

        public static Color SetAlpha(this Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }

        public static Vector3 ToVector3(this Vector2 v2, float z)
        {
            return new Vector3(v2.x, v2.y, z);
        }

        public static Vector3 ToVector3(this Vector2 v2)
        {
            return new Vector3(v2.x, v2.y, 0);
        }

        public static Vector2 ToVector2(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public static Vector3 SetZ(this Vector3 v3, float z)
        {
            v3.z = z;
            return v3;
        }
        public static Vector3 SetY(this Vector3 v3, float y)
        {
            v3.y = y;
            return v3;
        }
        public static Vector3 SetX(this Vector3 v3, float x)
        {
            v3.x = x;
            return v3;
        }

        public static bool Contains(this LayerMask mask, int layer)
        {
            return (mask & (1 << layer)) != 0;
        }

        public static Vector2 GetAveragePoint(this Collision2D collision)
        {
            Vector2 point = Vector2.zero;

            int contactCount = collision.contactCount;
            for (int i = 0; i < contactCount; i++)
            {
                var contact = collision.GetContact(i);
                point += contact.point;
            }

            return point / contactCount;
        }

        public static Vector2 GetTotalImpulse(this Collision2D collision)
        {
            Vector2 impulse = Vector2.zero;

            int contactCount = collision.contactCount;
            for (int i = 0; i < contactCount; i++)
            {
                var contact = collision.GetContact(i);
                impulse += contact.normal * contact.normalImpulse;
                //impulse.x += contact.tangentImpulse * contact.normal.y;
                //impulse.y -= contact.tangentImpulse * contact.normal.x;
            }

            return impulse;
        }

        public static float GetYImpulse(this Collision2D collision)
        {
            float impulse = 0;

            int contactCount = collision.contactCount;
            for (int i = 0; i < contactCount; i++)
            {
                var contact = collision.GetContact(i);
                impulse += contact.normal.y * contact.normalImpulse;
            }

            return impulse;
        }

        public static List<Vector2> GetImpulses(this Collision2D collision)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);

            return contacts.Select(c => c.normal).ToList();
        }

        public static float Dot(this Vector2 v1, Vector2 v2)
        {
            return Vector2.Dot(v1, v2);
        }

        public static float Cross(this Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        public static Vector2 Multiply(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static float Sign(this float v)
        {
            return v == 0 ? 0 : Mathf.Sign(v);
        }

        public static float GetLength(this LineRenderer line)
        {
            Vector3[] vertices = new Vector3[line.positionCount];
            line.GetPositions(vertices);
            float length = 0f;
            for(int i=1; i<line.positionCount; i++)
            {
                length += (vertices[i] - vertices[i - 1]).magnitude;
            }
            return length;
        }

        public static T Top<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }
    }
}
