using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using System.Linq;

namespace engine {
    struct GameSprite : Drawable {
        public readonly List<RectangleShape> Images;
        public IntRect CollisionMask;
        public Vector2f Position;
        public float ImageSpeed;
        public float ImageIndex;

        private RectangleShape drawShape;

        public GameSprite(GameSprite original) {
            Images = original.Images.ToList();
            CollisionMask = new IntRect(
                original.CollisionMask.Left, original.CollisionMask.Top, 
                original.CollisionMask.Width, original.CollisionMask.Height
            );
            Position = new Vector2f(0, 0);
            ImageSpeed = original.ImageSpeed;
            ImageIndex = 0;
            drawShape = Images[0];
        }

        public GameSprite(List<RectangleShape> images, Vector2f center, IntRect collisionMask, float imageSpeed) {
            Images = images;
            CollisionMask = collisionMask;
            ImageSpeed = imageSpeed;

            for(int i = 0; i < Images.Count; i++) {
                Images[i].Origin = center;
            }

            Position = new Vector2f(0, 0);
            ImageIndex = 0;
            drawShape = Images[0];
        }
        
        public void Update(float deltaTime) {
            int imgIndexOld = (int) Math.Floor(ImageIndex);

            ImageIndex += ImageSpeed * deltaTime;
            if(ImageIndex > Images.Count) {
                ImageIndex = 0;
            }

            int imgIndexNew = (int) Math.Floor(ImageIndex);
            if(imgIndexNew != imgIndexOld) {
                drawShape = Images[imgIndexNew];
            }

            drawShape.Position = Position;
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Draw(drawShape);
        }

        public static List<RectangleShape> GetImagesFromTexture(Texture tex, IntRect[] cuts, Vector2f[] sizes) {
            var images = new List<RectangleShape>();

            for(int i = 0; i < cuts.Length; i++) {
                var cut = cuts[i];
                var size = sizes[i];

                var newImage = new RectangleShape(size);
                newImage.TextureRect = cut;
                newImage.Texture = tex;
                images.Add(newImage);
            }

            return images;
        }
    }

    class Sprites {
        private static Sprites instance = null;
        
        public static Sprites getInstance() {
            if(instance == null) {
                instance = new Sprites();
            }

            return instance;
        }

        public readonly Texture PlayerWalkRightTex;
        public readonly GameSprite PlayerWalkRight;

        private Sprites() {
            PlayerWalkRightTex = new Texture("img/basic-walk.png");
            PlayerWalkRight = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkRightTex,
                    new IntRect[] {
                        new IntRect(15, 206, 32, 48),
                        new IntRect(78, 206, 32, 48),
                        new IntRect(15, 206, 32, 48),
                        new IntRect(269, 206, 32, 48)
                    },
                    new Vector2f[] {
                        new Vector2f(64, 96),
                        new Vector2f(64, 96),
                        new Vector2f(64, 96),
                        new Vector2f(64, 96)
                    }
                ),
                new Vector2f(16, 24),
                new IntRect(0, 0, 32, 48),
                16
            );
        }
    }
}