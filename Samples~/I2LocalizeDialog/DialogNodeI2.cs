using UnityEngine;

namespace Reflectis.PLG.Dialogs
{
    public class DialogNodeI2 : DialogNode
    {
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the dialog text. If the system is using I2 localization, 
        /// it uses the dialog value as id to get the actual dialog text from I2's 
        /// database.</summary>
        public override string Dialog
        {
            get => I2.Loc.LocalizationManager.GetTranslation(dialog);
            set => dialog = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the character name. If the system is using I2 localization, 
        /// it uses the dialog value as id to get the actual character name from I2's 
        /// database.</summary>
        public override string Character
        {
            get => I2.Loc.LocalizationManager.GetTranslation(character);
            set => character = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the text label for the first option button.</summary>
        public override string Option1Label
        {
            get => I2.Loc.LocalizationManager.GetTranslation(option1Label);
            set => option1Label = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the text label for the second option button.</summary>
        public override string Option2Label
        {
            get => I2.Loc.LocalizationManager.GetTranslation(option2Label);
            set => option2Label = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the text label for the third option button.</summary>
        public override string Option3Label
        {
            get => I2.Loc.LocalizationManager.GetTranslation(option3Label);
            set => option3Label = value;
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>Returns the text label for the fourth option button.</summary>
        public override string Option4Label
        {
            get => I2.Loc.LocalizationManager.GetTranslation(option4Label);
            set => option4Label = value;
        }
    }
}
