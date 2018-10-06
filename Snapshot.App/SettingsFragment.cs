using Android.OS;
using Android.Support.V7.Preferences;

namespace Snapshot.App
{
    public class SettingsFragment : PreferenceFragmentCompat
    {
        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            AddPreferencesFromResource(Resource.Xml.prefs);
        }
    }
}