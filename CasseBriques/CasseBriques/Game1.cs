using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CasseBriques
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ObjetAnime briquebleue;
        ObjetAnime briquegrise;
        ObjetAnime unebriquenoire;
        ObjetAnime briqueorange;
        ObjetAnime briquepoint;
        ObjetAnime briquerouge;
        ObjetAnime briqueviolet;
        
        private SpriteFont textFont;
        private int TAILLEBRIQUEX;
        private int TAILLEBRIQUEY;
        private int NBBRIQUES = 8;
        private int NBLIGNES = 5;
        
        private Balle uneballe;
        private Raquette raquette;
        private int TAILLEH;
        private int TAILLEV;
        private Brique[,] mesBriques;
        private int offsetX;
        private int offsetY;
        private Song musique;
        private Texture2D fond;
        private static Boolean aGagne = false;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            int offsetX = 40;
            int offsetY = 40;
            TAILLEH = 1024; // this.GraphicsDevice.Viewport.Width à mettre + tard
            TAILLEV = 660; //this.GraphicsDevice.Viewport.Height à mettre + tard
            TAILLEBRIQUEX = 119; //trouver la bonne équation à mettre
            TAILLEBRIQUEY = 50; // same

            // TODO: Add your initialization logic here
            uneballe = new Balle(this, TAILLEH, TAILLEV);
            raquette = new Raquette(this, TAILLEH, TAILLEV);
            uneballe.Raquette = raquette;
            raquette.Balle = uneballe;
            mesBriques = new Brique[NBLIGNES, NBBRIQUES];
            // On passe à la balle le tableau de briques
            int xpos, ypos;
            for (int x = 0; x < NBLIGNES; x++)
            {
                ypos = offsetY + x * TAILLEBRIQUEY;
                for (int y = 0; y < NBBRIQUES; y++)
                {
                    xpos = offsetX + y * TAILLEBRIQUEX;

                    Vector2 pos = new Vector2(xpos, ypos);
                    // On mémorise les positions de la brique
                    mesBriques[x, y] = new Brique(this, pos, new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY));
                }
            }

            uneballe.MesBriquesballe = mesBriques;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferredBackBufferWidth = TAILLEH;
            graphics.PreferredBackBufferHeight = TAILLEV;
            graphics.ApplyChanges();

            // TODO: use this.Content to load your game content here


            /// On initialise les différents objets du jeu
            /// lignes de briques
            /// la balle
            /// la raquette
            /// 
            briquegrise = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\brique0"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briquebleue = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\brique1"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briqueorange = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\brique3"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briquepoint = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\brique4"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briquerouge = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\brique5"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            briqueviolet = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\brique6"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            unebriquenoire = new ObjetAnime(Content.Load<Texture2D>(@"mesimages\briquetransparente"), new Vector2(0f, 0f), new Vector2(TAILLEBRIQUEX, TAILLEBRIQUEY), Vector2.Zero);
            fond = Content.Load<Texture2D>(@"mesImages\fond2");
            musique = Content.Load <Song>(@"sounds\musique2");
            MediaPlayer.Play(musique);
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.IsRepeating = true;
            // On charge la police

            this.textFont = Content.Load<SpriteFont>(@"font\aFont");



            // TODO: use this.Content to load your game content here
        }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector2 pos;
            GraphicsDevice.Clear(Color.Black);
            drawBgMotif(fond);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // Boucle permettant de dessiner les briques des murs
            spriteBatch.DrawString(this.textFont, "Score : " + uneballe.Score, new Vector2(900,500), Color.White);
            spriteBatch.DrawString(this.textFont, "Vies : " + uneballe.Nombreballes, new Vector2(900, 460), Color.White);
            drawPartie();
            if (uneballe.Nombreballes == 0)
            {
                if (!aGagne)
                spriteBatch.DrawString(this.textFont, "Game Over ... ! Vous avez epuise toutes vos balles. Votre score est :" + uneballe.Score, new Vector2(13 * 20, 18 * 20), Color.Yellow);
                redemarrage();
                    
            }
            for (int x = 0; x < NBLIGNES; x++)
            {
                for (int y = 0; y < NBBRIQUES; y++)
                {
                    
                    pos = mesBriques[x, y].Position;
                    if (!mesBriques[x, y].Marque)
                        switch (x)
                        {
                            case 0: spriteBatch.Draw(briquepoint.Texture, pos, Color.Azure); break;
                            case 1: spriteBatch.Draw(briquegrise.Texture, pos, Color.Gray); break;
                            case 2: spriteBatch.Draw(briquerouge.Texture, pos, Color.Red); break;
                            case 3: spriteBatch.Draw(briqueorange.Texture, pos, Color.Orange); break;
                            case 4: spriteBatch.Draw(briqueviolet.Texture, pos, Color.Violet); break;
                        }
                    else
                    {
                        spriteBatch.Draw(unebriquenoire.Texture, pos, Color.Black);
                    }

                

                }
            }
            if (uneballe.Compteur == NBLIGNES * NBBRIQUES)
            {


                spriteBatch.DrawString(this.textFont, "Bravo, vous avez gagne ! Votre score est :" + uneballe.Score, new Vector2(13 * 20, 18 * 20), Color.Yellow);
                aGagne = true;
                redemarrage();
            }
                spriteBatch.End();
            base.Draw(gameTime);
        }
        private void drawBgMotif(Texture2D motif)
        {
            int nbMotifsLargeur = this.GraphicsDevice.Viewport.Width / motif.Width + 1;
            Console.WriteLine("nbMotifsLargeur : " + nbMotifsLargeur);
            int nbMotifsHauteur = this.GraphicsDevice.Viewport.Height / motif.Height + 1;
            Console.WriteLine("nbMotifsLargeur : " + nbMotifsLargeur);

            spriteBatch.Begin();
            for (int i = 0; i < nbMotifsLargeur; i++)
            {
                for (int j = 0; j < nbMotifsHauteur; j++)
                    spriteBatch.Draw(this.fond,
                                    new Vector2(i * motif.Width, j * motif.Height),
                                    Color.Azure);
            }
            spriteBatch.End();
        }
        private void drawPartie()
        {
            if (!uneballe.EstDemarre)
            {
                spriteBatch.DrawString(this.textFont, "Appuyez sur la barre d'espace pour lancer la balle", new Vector2(TAILLEH/4, TAILLEV/2), Color.White);
            }
        }

       private void redemarrage()
        {
            uneballe.EstDemarre = false;
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Space))
            {
                uneballe.Compteur = 0;
                uneballe.Score = 0;
                uneballe.Nombreballes = 3;
                raquette.Uneraquette.Texture = this.Content.Load<Texture2D>(@"mesImages\raquette3");
                raquette.TailleX = 90;
                uneballe.EstDemarre = false;
                uneballe.Uneballe.Position = uneballe.PositionDep;
                uneballe.Uneballe.Vitesse = Vector2.Zero;
                uneballe.EstDemarre = false;
                for (int x = 0; x < NBLIGNES; x++)
                {
                    for (int y = 0; y < NBBRIQUES; y++)
                    {
                        mesBriques[x, y].Marque = false;
                    }
                }
            }
        }

        public static Boolean Agagne
        {
            get { return aGagne; }
            set { aGagne = value; }
        }
    }
}
