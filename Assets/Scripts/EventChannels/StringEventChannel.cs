using UnityEngine;

namespace Firefly.EventChannels
{
    [CreateAssetMenu(menuName = "EventChannels/StringEventChannel")]
    public class StringEventChannel : GenericEventChannel<string> { }
}