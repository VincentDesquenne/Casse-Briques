using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace CasseBriques
{
    class Controls
    {
        private const Keys TOUCHE_DROITE = Keys.Right;
        private const Keys TOUCHE_GAUCHE = Keys.Left;

        // Vérifie si le joueur  a effectué l'action "aller à droite"
        public static Boolean CheckActionDroite()
        {
            Boolean checkActiondown = false;
            KeyboardState keyboard = Keyboard.GetState();

            checkActiondown = keyboard.IsKeyDown(TOUCHE_DROITE);

            return checkActiondown;
        }

        // Vérifie si le joueur a effectué l'action "aller à gauche"
        public static Boolean CheckActionGauche()
        {
            Boolean checkActionDown = false;
            KeyboardState keyboard = Keyboard.GetState();

            checkActionDown = keyboard.IsKeyDown(TOUCHE_GAUCHE);

            return checkActionDown;
        }


    }
}
