using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Drawing;

namespace GameMain
{
    public class GameMainConst
    {
        /// <summary>
        /// レア度
        /// </summary>
        public enum Rare
        {
            NONE = 0,
            SR,
            SSR,
            UR,
        }

        /// <summary>
        /// レア度リスト取得
        /// </summary>
        /// <returns>レア度リスト</returns>
        public static List<Rare> GetRareList() {
            List<Rare> rareList = Enum.GetValues(typeof(Rare))
                .Cast<Rare>()
                .ToList();

            return rareList;
        }

        /// <summary>
        /// レア度名取得
        /// </summary>
        /// <param name="scene">レア度</param>
        /// <returns>レア度名</returns>
        public static string GetRareName(Rare rare) {
            Dictionary<Rare, string> rareNameDictionary = new() {
                {Rare.NONE, "ハズレ"},
                {Rare.SR, "SR"},
                {Rare.SSR, "SSR"},
                {Rare.UR, "UR"},
            };

            return rareNameDictionary[rare];
        }

        /// <summary>
        /// 設定
        /// </summary>
        public class Config
        {
            public int PointPerTime  { get; set; }
            public bool IsUprate { get; set; }
            public int UpratePoint { get; set; }
            public float UprateRate { get; set; }
            public List<RateSetting> BeforeRateSettingList { get; set; }
            public List<RateSetting> AfterRateSettingList { get; set; }

            /// <summary>
            /// 確率設定
            /// </summary>
            public class RateSetting
            {
                public Rare Rare { get; set; }
                public float Rate { get; set; }
            }

            /// <summary>
            /// 初期設定取得
            /// </summary>
            /// <returns>初期設定リスト</returns>
            public static Config GetInitConfig() {
                var InitConfig = new Config {
                    PointPerTime = 10,
                    IsUprate = true,
                    UpratePoint = 500,
                    UprateRate = 0.05f,
                    BeforeRateSettingList = new List<RateSetting>() {
                        new() {Rare = Rare.UR, Rate = 0.1f},
                        new() {Rare = Rare.SSR, Rate = 0.3f},
                        new() {Rare = Rare.SR, Rate = 1.0f},
                        new() {Rare = Rare.NONE, Rate = 98.6f},
                    },
                    // ディープコピーできなかったので
                    AfterRateSettingList = new List<RateSetting>() {
                        new() {Rare = Rare.UR, Rate = 0.1f},
                        new() {Rare = Rare.SSR, Rate = 0.3f},
                        new() {Rare = Rare.SR, Rate = 1.0f},
                        new() {Rare = Rare.NONE, Rate = 98.6f},
                    },
                };

                return InitConfig;
            }
        }

        /// パーセント最大値
        public static readonly float PERCENT_MAX = 100.0f;
    }
}