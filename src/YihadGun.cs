﻿using DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMod.src
{
    [EditorGroup("KzMod|YihadGun")]
    public class YihadGun : Grenade
    {


        public SpriteMap sprite;
        private bool hasImpacted = false;
        private Duck previousOwner;
        private ExplosiveVest vest;

        public YihadGun(float x, float y) : base(x, y)
        {
            sprite = new SpriteMap(GetPath("YihadGun"), 32, 32, false);
            graphic = sprite;
            this.sprite.frame = 0;
            _editorName = "Yihad Gun";
            collisionSize = new Vec2(9f, 14f);
            collisionOffset = new Vec2(-5f, -7f);
            center = new Vec2(16f, 16f);
            _barrelOffsetTL = new Vec2(16f, 10f);
        }

        public override void Update()
        {
            this._timer = 1;

            if (previousOwner == null && this.owner as Duck != null)
            {
                this.previousOwner = this.owner as Duck;
            }
            if (this.hasImpacted)
            {
                this.sprite.frame = 1;
            }
            base.Update();
        }


        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {

            if (with as Duck != null && !_pin && !hasImpacted && with != this.previousOwner)
            {
                var duck = (Duck)with;
                ExplosiveVest vest = (ExplosiveVest)Editor.CreateThing(typeof(ExplosiveVest));
                vest.x = duck.x;
                vest.y = duck.y;
                vest.detonator = this;
                Level.Add(vest);
                this.vest = (ExplosiveVest) vest;
                duck.Equip((Equipment) vest);
                duck.GiveHoldable(this);
                hasImpacted = true;
            }
        }

        public override void OnPressAction()
        {
            if (this.hasImpacted)
            {
                this.vest.AllahuAkhbar();
                Level.Remove(this);
            }
            base.OnPressAction();
        }
    }
}
