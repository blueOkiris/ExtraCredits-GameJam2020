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

        void Init();
        void Update(float deltaTime, KeyState keys);
    }

    class Player : GameObject
    {
        private Vector2f acc;
        private Vector2f vel;
        private Vector2f pos;

        private GameSprite spriteIndex;

        public Player(Vector2f defaultPos) {
            spriteIndex = new GameSprite(Sprites.getInstance().PlayerWalkRight);
            pos = defaultPos;

            vel = new Vector2f(512, 0);
            acc = new Vector2f(0, 0);
        }

        public void Init() {
            
        }

        public void Update(float deltaTime, KeyState keys) {
            spriteIndex.Position = pos;
            spriteIndex.Update(deltaTime);

            if(pos.X > Settings.ScreenSize.X) {
                pos.X = 0;
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
