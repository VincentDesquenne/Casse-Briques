using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CasseBriques
{
    public class Balle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        
        private int Nbreballes;
        private int maxX, minX;
        private int maxY, minY;
        private Vector2 v_min, v_max;
        private Vector2 vitesse_initiale, position_depart;
        private SpriteBatch spriteBatch;
        private ObjetAnime uneballe;
        private int TAILLEX = 5, TAILLEY=5;
        private Vector2 v;
        private Boolean collision_murs;
        private BoundingBox bbox;
        private Brique[,] mesBriquesballe;
        private int NBLIGNES = 5;
        private int NBCOLONNES = 8;
        private Raquette raquette;
        private ObjetAnime uneraquette;
        private SoundEffect soundRaquette;
        private SoundEffect soundMur;
        private SoundEffect soundBrique;
        private Boolean estDemarre = false;
        private int score = 0;
        private int count = 0;
        private Boolean collx = false;

        public Balle(Game game, int th, int tv)
            : base(game)
        {
            /// On récupère la taille de sortie de l'écran 

            maxX = th;
            maxY = tv;
            this.Nbreballes = 3;
            this.Game.Components.Add(this);
                       
            
        }

        
        public override void Initialize()
        {
            // On définit une vitesse initiale minimale
            Random r = new Random();
            int randomNumberX = r.Next(-5,5);
            

            v_min = new Vector2(randomNumberX, -4);
            // on fixe une vitesse maximale
            v_max = new Vector2(6, 6);
            this.vitesse_initiale = new Vector2(0,0);
            // On place la balle au centre de l'écran au dessus de la raquette 
            this.position_depart = new Vector2((maxX / 2) - 10, (maxY - 50));
            minX = 0;
            minY = 0;
            //On fixe le nombre de balles
            
            base.Initialize();
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void LoadContent()
        {
            
            try
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);
                
               
            }
            catch(OutOfMemoryException e)
            {
                Console.WriteLine(e.Message);
            }
                      
                                    
                       
                        
            uneballe = new ObjetAnime(Game.Content.Load<Texture2D>(@"mesimages\balle3"), this.position_depart, new Vector2(TAILLEX, TAILLEY), this.vitesse_initiale);
            soundRaquette = Game.Content.Load<SoundEffect>(@"sounds\collisionRaquette");
            soundMur = Game.Content.Load<SoundEffect>(@"sounds\collisionMur");
            soundBrique = Game.Content.Load<SoundEffect>(@"sounds\collisionBrique");
            // on met à jour la Bounding Box
            
            base.LoadContent();
            
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(uneballe.Texture, uneballe.Position, Color.Azure);
            spriteBatch.End();           
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.bbox = new BoundingBox(new Vector3(uneballe.Position.X, uneballe.Position.Y, 0),
                new Vector3(uneballe.Position.X + uneballe.Texture.Width, uneballe.Position.Y + uneballe.Texture.Height, 0));
            Demarrage();
            BougeBalle();
            
            base.Update(gameTime);


        }

        public void Demarrage()
        {
            if (!estDemarre)
            {
                KeyboardState keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(Keys.Space))
                {
                    
                    v = v_min;
                    estDemarre = true;
                    uneballe.Vitesse = v;
                   
                }
            }
        }

        public void BougeBalle()
        {
            // avec les murs
            bool collision_murs = false;
            v = uneballe.Vitesse;
            // collision avec le mur gauche
            if (uneballe.Position.X + uneballe.Size.X <= minX)
            {
                v.X *= -1;
                uneballe.Vitesse = v;
                collision_murs = true;
            }
            // collision avec le mur droit
            if (uneballe.Position.X >= maxX)
            {
                v.X *= -1;
                uneballe.Vitesse = v;
                collision_murs = true;
            }
            // On passe au dessus du nur 
            if (uneballe.Position.Y + uneballe.Size.Y <= minY)
            {
                v.Y *= -1;
                collision_murs = true;
                uneballe.Vitesse = v;
            }

            // on perd la balle 
            if (uneballe.Position.Y >= maxY)
            {
                
                collision_murs = true;
                uneballe.Vitesse = this.vitesse_initiale;                
                uneballe.Position = this.position_depart;
                this.Nbreballes--;
                raquette.TailleX -= 30;
                if (!Game1.Agagne)
                score -= 25;
                estDemarre = false;
                if (Nbreballes == 2)
                    raquette.Uneraquette.Texture = Game.Content.Load<Texture2D>(@"mesImages\raquettePlusPetite");
                if (Nbreballes == 1)
                    raquette.Uneraquette.Texture = Game.Content.Load<Texture2D>(@"mesImages\raquetteVPlusPetite");
                
            }

            if (collision_murs)
            {
            SoundEffectInstance soundInstMur = soundMur.CreateInstance();
            soundInstMur.Volume = 0.6f;
            soundInstMur.Play();


                       }
            gestionCollision();
            uneballe.Position += uneballe.Vitesse;
            
        }

        public BoundingBox Bbox
        {
            get { return bbox; }
            set { bbox = value; }
        }

        private void gestionCollision()
        {
            Vector2 v;
            // Test de collision
            float[] infosBalle = { uneballe.Position.X, uneballe.Position.Y, TAILLEX, TAILLEY };
            int[] posRel;
            // avec les raquettes
            // On récupère la vitesse courante
            v = uneballe.Vitesse;
            if (Moteur2D.testCollision(this, this.raquette.Bbox))
            {
                // Le prochain mouvement entraîne une collision, on évalue la position relative de la balle
                // par rapport à la raquette pour mettre à jour le vecteur vitesse
                float[] infosRaquette = { raquette.Uneraquette.Position.X, raquette.Uneraquette.Position.Y, raquette.Uneraquette.Size.X, raquette.Uneraquette.Size.Y };
                posRel = Moteur2D.getRelativePosition(infosBalle, infosRaquette);

                if (posRel[0] == Moteur2D.CROISEMENT)
                {
                    v.Y *= -1;
                    
                    if (Math.Abs(v.Y) < v_max.Y)
                        v.Y *= 1.2f;
                    uneballe.Vitesse = v;

                }

                // Si les 2 objets se croisent sur l'axe des Y
                if (posRel[1] == Moteur2D.CROISEMENT)
                {
                    v.X *= -1;
                    if (Math.Abs(v.X) < v_max.X)
                        v.X *= 1.2f;
                    uneballe.Vitesse = v;
                }

                SoundEffectInstance soundInstRaquette = soundRaquette.CreateInstance();
                soundInstRaquette.Volume = 0.3f;
                soundInstRaquette.Play();
            }

            
            gestionCollisionBrique();
            
        }
        public Brique[,] MesBriquesballe
        {
            get { return mesBriquesballe; }
            set { mesBriquesballe = value; }

        }

        // Test la collision avec les briques 
        private void gestionCollisionBrique()
        {
            BoundingBox bbox_brique;
            Brique unebrique;
            Vector2 v;
            Boolean collision = false;
            float[] infosBalle = { uneballe.Position.X, uneballe.Position.Y, uneballe.Position.X + TAILLEX, uneballe.Position.Y + TAILLEY };
            int[] posRel;
            int x = 0;
            int y = 0;
            int tempx = 0, tempy = 0;
            v = uneballe.Vitesse;
            // on teste une collision avec une éventuelle brique
            while (x < NBLIGNES && !collision)
            {
                y = 0;
                while (y < NBCOLONNES && !collision)
                {
                    unebrique = mesBriquesballe[x, y];
                    // On définit uen enveloppe pour la brique
                    bbox_brique = new BoundingBox(new Vector3(unebrique.Position.X, unebrique.Position.Y, 0),
                                 new Vector3(unebrique.Position.X + unebrique.Size.X, unebrique.Position.Y + unebrique.Size.Y, 0));


                    // on mémorise l'enveloppe
                    if (!unebrique.Marque && Moteur2D.testCollision(this, bbox_brique))
                    {
                        // Le prochain mouvement entraîne une collision, on évalue la position 
                        // relative de la balle
                        // par rapport à la raquette pour mettre à jour le vecteur vitesse
                        float[] infosBrique = { unebrique.Position.X, unebrique.Position.Y, unebrique.Position.X + unebrique.Size.X, unebrique.Position.Y + unebrique.Size.Y };
                        posRel = Moteur2D.getRelativePosition(infosBalle, infosBrique);

                        if (posRel[0] == Moteur2D.CROISEMENT && (posRel[1] == Moteur2D.AU_DESSUS || posRel[1] == Moteur2D.EN_DESSOUS))
                        {
                            v.Y *= -1;
                           
                            collx = true;
                            if (Math.Abs(v.Y) < v_max.Y)
                                v.Y *= 1.1f;
                            uneballe.Vitesse = v;

                        }

                        // Si les 2 objets se croisent sur l'axe des Y
                        else if (posRel[1] == Moteur2D.CROISEMENT)
                        {

                            v.X *= -1;
                            
                            if (Math.Abs(v.X) < v_max.X)
                                v.X *= 1.1f;
                            uneballe.Vitesse = v;
                        }
                        
                        SoundEffectInstance soundInstBrique = soundBrique.CreateInstance();
                        soundInstBrique.Volume = 0.8f;
                        soundInstBrique.Play();
                        collision = true;
                        tempx = x;
                        tempy = y;
                        score += 10;
                    }
                    y++;
                }
                x++;
            }
            if (collision)
            {
                mesBriquesballe[tempx, tempy].Marque = true;
                count++;
            }
            
                
        }

        public Raquette Raquette
        {
            get { return raquette; }
            set { raquette = value; }
        }

        public ObjetAnime Uneballe
        {
            get { return uneballe; }
            set { uneballe = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public Boolean EstDemarre
        {
            get { return estDemarre; }
            set { estDemarre = value; }
        }

        public int Nombreballes
        {
             get { return Nbreballes; }
             set { Nbreballes = value; }

        }

        public int Compteur
        {
            get { return count; }
            set { count = value; }
        }

        public Vector2 PositionDep
        {
            get { return position_depart; }
            set { position_depart = value; }
        }
    } 
}



