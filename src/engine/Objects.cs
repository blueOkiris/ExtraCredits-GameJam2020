using System;
using SFML.Graphics;
using SFML.System;

namespace engine {
    interface GameObject : Drawable {
        Vector2f GetAcceleration();
        Vector2f GetVelocity();
        Vector2f GetPosition();
        void SetAcceleration(Vector2f acc);
        void SetVelocity(Vector2f vel);
        void SetPosition(Vector2f pos);
        
        GameSprite GetSpriteIndex();
        IntRect GetMask();
        void SetSpriteIndex(GameSprite spr);
        void SetMask(IntRect mask);

        string GetTag();

        void Init();
        void Update(float deltaTime, KeyState keys, Room room);
    }

    class TestPlayer : GameObject
    {
        private Vector2f acc;
        private Vector2f vel;
        private Vector2f pos;

        private GameSprite spriteIndex;
        private float moveSpeed;
        private bool facingLeft;

        public string GetTag() => "player";

        public TestPlayer(Vector2f defaultPos) {
            spriteIndex = new GameSprite(Sprites.getInstance().PlayerStandRight);
            pos = defaultPos;

            vel = new Vector2f(0, 0);
            acc = new Vector2f(0, 0);

            moveSpeed = 512;
            facingLeft = false;
        }

        public void Init() {
            
        }

        public void Update(float deltaTime, KeyState keys, Room room) {
            spriteIndex.Position = pos;
            spriteIndex.Update(deltaTime);
            
            if(keys.Up && pos.Y > 0) {
                if(vel.Y != -moveSpeed) {
                    vel.Y = -moveSpeed;

                    if(facingLeft) {
                        spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkLeft);
                    } else {
                        spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkRight);
                    }
                }
            } else if(keys.Down && pos.Y < room.GetSize().Y) {
                if(vel.Y != moveSpeed) {
                    vel.Y = moveSpeed;

                    if(facingLeft) {
                        spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkLeft);
                    } else {
                        spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkRight);
                    }
                }
            } else {
                vel.Y = 0;
            }

            if(keys.Left && pos.X > 0) {
                if(vel.X != -moveSpeed) {
                    vel.X = -moveSpeed;
                    spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkLeft);
                }

                facingLeft = true;
            } else if(keys.Right && pos.X < room.GetSize().X) {
                if(vel.X != moveSpeed) {
                    vel.X = moveSpeed;
                    spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkRight);
                }
                
                facingLeft = false;
            } else {
                vel.X = 0;
            }

            if(vel.X == 0 && vel.Y == 0) {
                if(facingLeft) {
                    spriteIndex = new GameSprite(Sprites.getInstance().PlayerStandLeft);
                } else {
                    spriteIndex = new GameSprite(Sprites.getInstance().PlayerStandRight);
                }
            }
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Draw(spriteIndex);
        }

        public Vector2f GetAcceleration() => acc;
        public Vector2f GetVelocity() => vel;
        public Vector2f GetPosition() => pos;
        public void SetAcceleration(Vector2f acc) => this.acc = acc;
        public void SetVelocity(Vector2f vel) => this.vel = vel;
        public void SetPosition(Vector2f pos) => this.pos = pos;

        public GameSprite GetSpriteIndex() => spriteIndex;
        public IntRect GetMask() => spriteIndex.CollisionMask;
        public void SetSpriteIndex(GameSprite spr) => this.spriteIndex = spr;
        public void SetMask(IntRect mask) {}
    }
}
