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
        public Vector2f Origin;
        public float ImageSpeed;
        public float ImageIndex;

        private RectangleShape drawShape;

        public GameSprite(GameSprite original) {
            Images = new List<RectangleShape>();

            foreach(var image in original.Images) {
                RectangleShape newImage = new RectangleShape(image.Size);
                newImage.Origin = image.Origin;
                newImage.Texture = image.Texture;
                newImage.TextureRect = image.TextureRect;

                Images.Add(newImage);
            }

            CollisionMask = new IntRect(
                original.CollisionMask.Left, original.CollisionMask.Top, 
                original.CollisionMask.Width, original.CollisionMask.Height
            );
            Position = new Vector2f(0, 0);
            ImageSpeed = original.ImageSpeed;
            ImageIndex = 0;
            drawShape = Images[0];
            Origin = drawShape.Origin;
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
            Origin = drawShape.Origin;
        }
        
        public void Update(float deltaTime) {
            int imgIndexOld = (int) Math.Floor(ImageIndex);

            ImageIndex += ImageSpeed * deltaTime;
            if(ImageIndex >= Images.Count) {
                ImageIndex = 0;
            }

            int imgIndexNew = (int) Math.Floor(ImageIndex);
            if(imgIndexNew != imgIndexOld) {
                drawShape = Images[imgIndexNew];
            }

            drawShape.Position = Position;
            Origin = drawShape.Origin;
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

        public readonly Texture Blank;
        public readonly Texture PlayerWalkTex;
        public readonly Texture SimpleGrassBlockTex;
        public readonly GameSprite Empty;
        public readonly GameSprite PlayerWalkRight, PlayerWalkLeft;
        public readonly GameSprite PlayerStandRight, PlayerStandLeft;
        public readonly GameSprite SimpleGrassBlock;

        private Sprites() {
            Blank = new Texture("img/blank.png");
            Empty = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    Blank, 
                    new IntRect[] { new IntRect(0, 0, 64, 64) }, 
                    new Vector2f[] {
                        new Vector2f(64, 64)
                    }
                ),
                new Vector2f(0, 0),
                new IntRect(0, 0, 64, 64),
                0
            );

            PlayerWalkTex = new Texture("img/sammy-move.png");
            PlayerWalkRight = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(1, 1, 32, 32),
                        new IntRect(34, 1, 32, 32),
                        new IntRect(67, 1, 32, 32),
                        new IntRect(100, 1, 32, 32)
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                        new Vector2f(128, 128),
                        new Vector2f(128, 128),
                        new Vector2f(128, 128)
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                12
            );
            PlayerWalkLeft = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(100, 34, 32, 32),
                        new IntRect(67, 34, 32, 32),
                        new IntRect(34, 34, 32, 32),
                        new IntRect(1, 34, 32, 32)
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                        new Vector2f(128, 128),
                        new Vector2f(128, 128),
                        new Vector2f(128, 128)
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                12
            );
            PlayerStandRight = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(1, 1, 32, 32),
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );
            PlayerStandLeft = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    PlayerWalkTex,
                    new IntRect[] {
                        new IntRect(100, 34, 32, 32),
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128),
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );

            SimpleGrassBlockTex = new Texture("img/simple-dirt.png");
            SimpleGrassBlock = new GameSprite(
                GameSprite.GetImagesFromTexture(
                    SimpleGrassBlockTex,
                    new IntRect[] {
                        new IntRect(0, 0, 32, 32)
                    },
                    new Vector2f[] {
                        new Vector2f(128, 128)
                    }
                ),
                new Vector2f(64, 64),
                new IntRect(0, 0, 128, 128),
                0
            );
        }
    }
}