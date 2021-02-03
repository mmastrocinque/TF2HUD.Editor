﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using TF2HUD.Editor.Common;
using TF2HUD.Editor.Properties;

namespace TF2HUD.Editor.HUDs
{
    /// <summary>
    ///     Interaction logic for FlawHUD.xaml
    /// </summary>
    public partial class FlawHUD
    {
        public FlawHUD()
        {
            InitializeComponent();
            ReloadHudSettings();
        }

        /// <summary>
        ///     Disable crosshair options if the crosshair is toggled off.
        /// </summary>
        private void SetCrosshairControls()
        {
            CbXHairHitmarker.IsEnabled = CbXHairEnable.IsChecked ?? false;
            CbXHairRotate.IsEnabled = CbXHairEnable.IsChecked ?? false;
            CpXHairColor.IsEnabled = CbXHairEnable.IsChecked ?? false;
            CpXHairPulse.IsEnabled = CbXHairEnable.IsChecked ?? false;
            IntXHairSize.IsEnabled = CbXHairEnable.IsChecked & !CbXHairRotate.IsChecked ?? false;
            CbXHairStyle.IsEnabled = CbXHairEnable.IsChecked & !CbXHairRotate.IsChecked ?? false;
            CbXHairEffect.IsEnabled = CbXHairEnable.IsChecked & !CbXHairRotate.IsChecked ?? false;
        }

        /// <summary>
        ///     Used to determine where to position the item effect meters.
        /// </summary>
        private static void SetItemEffectPosition(string file,
            Positions position = Positions.Bottom,
            string search = "HudItemEffectMeter")
        {
            // positions 1 = top, 2 = middle, 3 = bottom
            var lines = File.ReadAllLines(file);
            var start = Utilities.FindIndex(lines, search);
            var value = position switch
            {
                Positions.Top => flawhud.Default.toggle_lower_stats ? "r70" : "c100",
                Positions.Middle => flawhud.Default.toggle_lower_stats ? "r60" : "c110",
                Positions.Bottom => flawhud.Default.toggle_lower_stats ? "r50" : "c120",
                _ => flawhud.Default.toggle_lower_stats ? "r80" : "c92"
            };
            lines[Utilities.FindIndex(lines, "ypos", start)] = $"\t\t\"ypos\"\t\t\t\t\"{value}\"";
            File.WriteAllLines(file, lines);
        }

        #region CLICK_EVENTS

        /// <summary>
        ///     Disable crosshair options if the crosshair is toggled on.
        /// </summary>
        private void CbXHairEnable_OnClick(object sender, RoutedEventArgs e)
        {
            SetCrosshairControls();
        }

        /// <summary>
        ///     Disable the alternate stats option if lowered stats are enabled, and vice versa.
        /// </summary>
        private void CbMovePlayerStats_OnClick(object sender, RoutedEventArgs e)
        {
            if (CbLowerPlayerStats.IsChecked != null)
                CbAlternatePlayerStats.IsEnabled = !(bool) CbLowerPlayerStats.IsChecked;
            if (CbAlternatePlayerStats.IsChecked != null)
                CbLowerPlayerStats.IsEnabled = !(bool) CbAlternatePlayerStats.IsChecked;
        }

        private void btnHudsTF_Click(object sender, RoutedEventArgs e)
        {
            Utilities.OpenWebpage(Properties.Resources.url_flawhud_hudstf);
        }

        private void btnSteam_Click(object sender, RoutedEventArgs e)
        {
            Utilities.OpenWebpage(Properties.Resources.url_flawhud_steam);
        }

        #endregion CLICK_EVENTS

        #region SAVE_LOAD

        /// <summary>
        ///     Save user settings to file.
        /// </summary>
        public void SaveHudSettings()
        {
            try
            {
                var settings = flawhud.Default;
                settings.color_health_buff = CpHealthBuffed.SelectedColor?.ToString();
                settings.color_health_low = CpHealthLow.SelectedColor?.ToString();
                settings.color_ammo_low = CpAmmoLow.SelectedColor?.ToString();
                settings.color_ubercharge = CpUberBarColor.SelectedColor?.ToString();
                settings.color_xhair_normal = CpXHairColor.SelectedColor?.ToString();
                settings.color_xhair_pulse = CpXHairPulse.SelectedColor?.ToString();
                settings.color_target_health = CpTargetHealth.SelectedColor?.ToString();
                settings.color_target_damage = CpTargetDamage.SelectedColor?.ToString();
                settings.color_normal = CpItemNormal.SelectedColor?.ToString();
                settings.color_unique = CpItemUnique.SelectedColor?.ToString();
                settings.color_strange = CpItemStrange.SelectedColor?.ToString();
                settings.color_vintage = CpItemVintage.SelectedColor?.ToString();
                settings.color_haunted = CpItemHaunted.SelectedColor?.ToString();
                settings.color_genuine = CpItemGenuine.SelectedColor?.ToString();
                settings.color_collectors = CpItemCollectors.SelectedColor?.ToString();
                settings.color_unusual = CpItemUnusual.SelectedColor?.ToString();
                settings.color_community = CpItemCommunity.SelectedColor?.ToString();
                settings.color_valve = CpItemValve.SelectedColor?.ToString();
                settings.color_civilian = CpItemCivilian.SelectedColor?.ToString();
                settings.color_freelance = CpItemFreelance.SelectedColor?.ToString();
                settings.color_mercenary = CpItemMercenary.SelectedColor?.ToString();
                settings.color_commando = CpItemCommando.SelectedColor?.ToString();
                settings.color_assassin = CpItemAssassin.SelectedColor?.ToString();
                settings.color_elite = CpItemElite.SelectedColor?.ToString();
                settings.val_xhair_size = IntXHairSize.Value ?? 18;
                settings.val_xhair_style = CbXHairStyle.SelectedIndex;
                settings.val_xhair_effect = CbXHairEffect.SelectedIndex;
                settings.toggle_xhair_enable = CbXHairEnable.IsChecked ?? false;
                settings.toggle_xhair_pulse = CbXHairHitmarker.IsChecked ?? false;
                settings.toggle_xhair_rotate = CbXHairRotate.IsChecked ?? false;
                settings.toggle_disguise_image = CbDisguiseImage.IsChecked ?? false;
                settings.toggle_stock_backgrounds = CbDefaultBg.IsChecked ?? false;
                settings.toggle_menu_images = CbMenuImages.IsChecked ?? false;
                settings.toggle_transparent_viewmodels = CbTransparentViewmodel.IsChecked ?? false;
                settings.toggle_code_fonts = CbCodeProFonts.IsChecked ?? false;
                settings.toggle_lower_stats = CbLowerPlayerStats.IsChecked ?? false;
                settings.toggle_alt_stats = CbAlternatePlayerStats.IsChecked ?? false;
                settings.val_health_style = CbHealthStyle.SelectedIndex;
                settings.val_killfeed_rows = IntKillFeedRows.Value ?? 5;
                settings.Save();
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error,
                    string.Format(Properties.Resources.error_app_save, MainWindow.HudSelection), ex.Message);
            }
        }

        /// <summary>
        ///     Load user settings from file.
        /// </summary>
        public void ReloadHudSettings()
        {
            try
            {
                var settings = flawhud.Default;
                var cc = new ColorConverter();
                CpHealthBuffed.SelectedColor = (Color) cc.ConvertFrom(settings.color_health_buff);
                CpHealthLow.SelectedColor = (Color) cc.ConvertFrom(settings.color_health_low);
                CpAmmoLow.SelectedColor = (Color) cc.ConvertFrom(settings.color_ammo_low);
                CpUberBarColor.SelectedColor = (Color) cc.ConvertFrom(settings.color_ubercharge);
                CpXHairColor.SelectedColor = (Color) cc.ConvertFrom(settings.color_xhair_normal);
                CpXHairPulse.SelectedColor = (Color) cc.ConvertFrom(settings.color_xhair_pulse);
                CpTargetHealth.SelectedColor = (Color) cc.ConvertFrom(settings.color_target_health);
                CpTargetDamage.SelectedColor = (Color) cc.ConvertFrom(settings.color_target_damage);
                CpItemNormal.SelectedColor = (Color)cc.ConvertFrom(settings.color_normal);
                CpItemUnique.SelectedColor = (Color)cc.ConvertFrom(settings.color_unique);
                CpItemStrange.SelectedColor = (Color)cc.ConvertFrom(settings.color_strange);
                CpItemVintage.SelectedColor = (Color)cc.ConvertFrom(settings.color_vintage);
                CpItemHaunted.SelectedColor = (Color)cc.ConvertFrom(settings.color_haunted);
                CpItemGenuine.SelectedColor = (Color)cc.ConvertFrom(settings.color_genuine);
                CpItemCollectors.SelectedColor = (Color)cc.ConvertFrom(settings.color_collectors);
                CpItemUnusual.SelectedColor = (Color)cc.ConvertFrom(settings.color_unusual);
                CpItemCommunity.SelectedColor = (Color)cc.ConvertFrom(settings.color_community);
                CpItemValve.SelectedColor = (Color)cc.ConvertFrom(settings.color_valve);
                CpItemCivilian.SelectedColor = (Color)cc.ConvertFrom(settings.color_civilian);
                CpItemFreelance.SelectedColor = (Color)cc.ConvertFrom(settings.color_freelance);
                CpItemMercenary.SelectedColor = (Color)cc.ConvertFrom(settings.color_mercenary);
                CpItemCommando.SelectedColor = (Color)cc.ConvertFrom(settings.color_commando);
                CpItemAssassin.SelectedColor = (Color)cc.ConvertFrom(settings.color_assassin);
                CpItemElite.SelectedColor = (Color)cc.ConvertFrom(settings.color_elite);
                IntXHairSize.Value = settings.val_xhair_size;
                CbXHairStyle.SelectedIndex = settings.val_xhair_style;
                CbXHairEffect.SelectedIndex = settings.val_xhair_effect;
                CbXHairEnable.IsChecked = settings.toggle_xhair_enable;
                CbXHairHitmarker.IsChecked = settings.toggle_xhair_pulse;
                CbXHairRotate.IsChecked = settings.toggle_xhair_rotate;
                CbDisguiseImage.IsChecked = settings.toggle_disguise_image;
                CbDefaultBg.IsChecked = settings.toggle_stock_backgrounds;
                CbMenuImages.IsChecked = settings.toggle_menu_images;
                CbTransparentViewmodel.IsChecked = settings.toggle_transparent_viewmodels;
                CbCodeProFonts.IsChecked = settings.toggle_code_fonts;
                CbLowerPlayerStats.IsChecked = settings.toggle_lower_stats;
                CbAlternatePlayerStats.IsChecked = settings.toggle_alt_stats;
                CbHealthStyle.SelectedIndex = settings.val_health_style;
                IntKillFeedRows.Value = settings.val_killfeed_rows;
                SetCrosshairControls();
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error,
                    string.Format(Properties.Resources.error_app_load, MainWindow.HudSelection), ex.Message);
            }
        }

        /// <summary>
        ///     Reset user settings to their default values.
        /// </summary>
        /// <remarks>TODO: Default settings should be read from a JSON file.</remarks>
        public void ResetHudSettings()
        {
            try
            {
                var cc = new ColorConverter();
                CpHealthBuffed.SelectedColor = (Color) cc.ConvertFrom("#00AA7F");
                CpHealthLow.SelectedColor = (Color) cc.ConvertFrom("#BE1414");
                CpAmmoLow.SelectedColor = (Color) cc.ConvertFrom("#BE1414");
                CpUberBarColor.SelectedColor = (Color) cc.ConvertFrom("#00AA7F");
                CpXHairColor.SelectedColor = (Color) cc.ConvertFrom("#F2F2F2");
                CpXHairPulse.SelectedColor = (Color) cc.ConvertFrom("#FF0000");
                CpTargetHealth.SelectedColor = (Color) cc.ConvertFrom("#00AA7F");
                CpTargetDamage.SelectedColor = (Color) cc.ConvertFrom("#FFFF00");
                CpItemNormal.SelectedColor = (Color)cc.ConvertFrom("#B2B2B2");
                CpItemUnique.SelectedColor = (Color)cc.ConvertFrom("#FFD700");
                CpItemStrange.SelectedColor = (Color)cc.ConvertFrom("#CF6A32");
                CpItemVintage.SelectedColor = (Color)cc.ConvertFrom("#476291");
                CpItemHaunted.SelectedColor = (Color)cc.ConvertFrom("#38F3AB");
                CpItemGenuine.SelectedColor = (Color)cc.ConvertFrom("#4D7455");
                CpItemCollectors.SelectedColor = (Color)cc.ConvertFrom("#AA0000");
                CpItemUnusual.SelectedColor = (Color)cc.ConvertFrom("#8650AC");
                CpItemCommunity.SelectedColor = (Color)cc.ConvertFrom("#70B04A");
                CpItemValve.SelectedColor = (Color)cc.ConvertFrom("#A50F79");
                CpItemCivilian.SelectedColor = (Color)cc.ConvertFrom("#B0C3D9");
                CpItemFreelance.SelectedColor = (Color)cc.ConvertFrom("#5E98D9");
                CpItemMercenary.SelectedColor = (Color)cc.ConvertFrom("#4B69FF");
                CpItemCommando.SelectedColor = (Color)cc.ConvertFrom("#8847FF");
                CpItemAssassin.SelectedColor = (Color)cc.ConvertFrom("#D32CE6");
                CpItemElite.SelectedColor = (Color)cc.ConvertFrom("#EB4B4B");
                IntXHairSize.Value = 18;
                CbXHairStyle.SelectedIndex = 24;
                CbXHairEffect.SelectedIndex = 0;
                CbXHairEnable.IsChecked = false;
                CbXHairHitmarker.IsChecked = true;
                CbXHairRotate.IsChecked = false;
                CbDisguiseImage.IsChecked = true;
                CbDefaultBg.IsChecked = false;
                CbMenuImages.IsChecked = true;
                CbTransparentViewmodel.IsChecked = false;
                CbCodeProFonts.IsChecked = false;
                CbLowerPlayerStats.IsChecked = false;
                CbAlternatePlayerStats.IsChecked = false;
                CbHealthStyle.SelectedIndex = 0;
                IntKillFeedRows.Value = 5;
                SetCrosshairControls();
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error,
                    string.Format(Properties.Resources.error_app_reset, MainWindow.HudSelection), ex.Message);
            }
        }

        /// <summary>
        ///     Apply user settings to the HUD files.
        /// </summary>
        public void ApplyHudSettings()
        {
            if (!MainMenuBackground()) return;
            if (!MainMenuClassImage()) return;
            if (!CrosshairRotate()) return;
            if (!Colors()) return;
            if (!CodeProFonts()) return;
            if (!HealthStyle()) return;
            if (!LowerPlayerStats()) return;
            if (!AlternatePlayerStats()) return;
            if (!Common.DisguiseImage()) return;
            if (!Common.Crosshair(CbXHairStyle.SelectedValue.ToString(), IntXHairSize.Value,
                CbXHairEffect.SelectedValue.ToString())) return;
            if (!Common.CrosshairPulse()) return;
            if (!Common.TransparentViewmodels()) return;
            if (!Common.ItemColors()) return;
            Common.KillFeedRows();
        }

        #endregion SAVE_LOAD

        #region CONTROLLER

        /// <summary>
        ///     Update the client scheme colors.
        /// </summary>
        private static bool Colors()
        {
            try
            {
                var file = string.Format(Properties.Resources.file_clientscheme_colors, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(file);
                // Health
                lines[Utilities.FindIndex(lines, "\"Overheal\"")] =
                    $"\t\t\"Overheal\"\t\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_health_buff)}\"";
                lines[Utilities.FindIndex(lines, "OverhealPulse")] =
                    $"\t\t\"OverhealPulse\"\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_health_buff, 200)}\"";
                lines[Utilities.FindIndex(lines, "\"LowHealth\"")] =
                    $"\t\t\"LowHealth\"\t\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_health_low)}\"";
                lines[Utilities.FindIndex(lines, "LowHealthPulse")] =
                    $"\t\t\"LowHealthPulse\"\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_health_low, 200)}\"";
                // Ammo
                lines[Utilities.FindIndex(lines, "\"LowAmmo\"")] =
                    $"\t\t\"LowAmmo\"\t\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_ammo_low)}\"";
                lines[Utilities.FindIndex(lines, "LowAmmoPulse")] =
                    $"\t\t\"LowAmmoPulse\"\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_ammo_low, 200)}\"";
                // Misc
                lines[Utilities.FindIndex(lines, "\"PositiveValue\"")] =
                    $"\t\t\"PositiveValue\"\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_health_buff)}\"";
                lines[Utilities.FindIndex(lines, "NegativeValue")] =
                    $"\t\t\"NegativeValue\"\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_health_low, 200)}\"";
                lines[Utilities.FindIndex(lines, "\"TargetHealth\"")] =
                    $"\t\t\"TargetHealth\"\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_target_health)}\"";
                lines[Utilities.FindIndex(lines, "TargetDamage")] =
                    $"\t\t\"TargetDamage\"\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_target_damage)}\"";
                // Crosshair
                lines[Utilities.FindIndex(lines, "\"Crosshair\"")] =
                    $"\t\t\"Crosshair\"\t\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_xhair_normal)}\"";
                lines[Utilities.FindIndex(lines, "CrosshairDamage")] =
                    $"\t\t\"CrosshairDamage\"\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_xhair_pulse)}\"";
                // ÜberCharge
                lines[Utilities.FindIndex(lines, "UberCharge")] =
                    $"\t\t\"UberCharge\"\t\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_ubercharge)}\"";
                lines[Utilities.FindIndex(lines, "UberChargePulse")] =
                    $"\t\t\"UberChargePulse\"\t\t\t\"{Utilities.RgbConverter(flawhud.Default.color_ubercharge, pulse: true)}\"";
                File.WriteAllLines(file, lines);
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_colors, ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Toggle health and ammo colors over text instead of a panel.
        /// </summary>
        private static bool ColorText(bool colorText = false)
        {
            try
            {
                var file = string.Format(Properties.Resources.file_hudanimations, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(file);
                // Panels
                Utilities.CommentOutTextLineSuper(lines, "HudHealthBonusPulse", "HealthBG", !colorText);
                Utilities.CommentOutTextLineSuper(lines, "HudHealthDyingPulse", "HealthBG", !colorText);
                Utilities.CommentOutTextLineSuper(lines, "HudLowAmmoPulse", "AmmoBG", !colorText);
                // Text
                Utilities.CommentOutTextLineSuper(lines, "HudHealthBonusPulse", "PlayerStatusHealthValue", colorText);
                Utilities.CommentOutTextLineSuper(lines, "HudHealthDyingPulse", "PlayerStatusHealthValue", colorText);
                Utilities.CommentOutTextLineSuper(lines, "HudLowAmmoPulse", "AmmoInClip", colorText);
                Utilities.CommentOutTextLineSuper(lines, "HudLowAmmoPulse", "AmmoInReserve", colorText);
                Utilities.CommentOutTextLineSuper(lines, "HudLowAmmoPulse", "AmmoNoClip", colorText);
                File.WriteAllLines(file, lines);
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_colors,
                    ex.Message); // TODO: Use a unique error message.
                return false;
            }
        }

        /// <summary>
        ///     Toggle the rotating crosshair.
        /// </summary>
        private static bool CrosshairRotate()
        {
            try
            {
                var file = string.Format(Properties.Resources.file_hudlayout, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(file);
                var start = Utilities.FindIndex(lines, "\"Crosshair\"");
                lines[Utilities.FindIndex(lines, "\"visible\"", start)] = "\t\t\"visible\"\t\t\t\"0\"";
                lines[Utilities.FindIndex(lines, "\"enabled\"", start)] = "\t\t\"enabled\"\t\t\t\"0\"";
                start = Utilities.FindIndex(lines, "\"CrosshairPulse\"");
                lines[Utilities.FindIndex(lines, "\"visible\"", start)] = "\t\t\"visible\"\t\t\t\"0\"";
                lines[Utilities.FindIndex(lines, "\"enabled\"", start)] = "\t\t\"enabled\"\t\t\t\"0\"";
                File.WriteAllLines(file, lines);

                if (!flawhud.Default.toggle_xhair_enable) return true;
                if (!flawhud.Default.toggle_xhair_rotate) return true;
                start = Utilities.FindIndex(lines, "\"Crosshair\"");
                lines[Utilities.FindIndex(lines, "\"visible\"", start)] = "\t\t\"visible\"\t\t\t\"1\"";
                lines[Utilities.FindIndex(lines, "\"enabled\"", start)] = "\t\t\"enabled\"\t\t\t\"1\"";
                start = Utilities.FindIndex(lines, "\"CrosshairPulse\"");
                lines[Utilities.FindIndex(lines, "\"visible\"", start)] = "\t\t\"visible\"\t\t\t\"1\"";
                lines[Utilities.FindIndex(lines, "\"enabled\"", start)] = "\t\t\"enabled\"\t\t\t\"1\"";
                File.WriteAllLines(file, lines);
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_xhair, ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Set the player health style.
        /// </summary>
        /// <remarks>TODO: Consider refactoring.</remarks>
        private static bool HealthStyle()
        {
            try
            {
                var file = string.Format(Properties.Resources.file_playerhealth, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(file);
                var start = Utilities.FindIndex(lines, "\"PlayerStatusHealthBonusImage\"");
                var index = Utilities.FindIndex(lines, "image", start);
                lines[index] = "\t\t\"image\"\t\t\t\"\"";

                if (!ColorText(flawhud.Default.val_health_style == 1)) return false;

                if (flawhud.Default.val_health_style == 2)
                {
                    lines[index] = "\t\t\"image\"\t\t\t\"../hud/health_over_bg\"";
                    File.WriteAllLines(file, lines);

                    file = string.Format(Properties.Resources.file_hudanimations, MainWindow.HudPath,
                        MainWindow.HudSelection);
                    lines = File.ReadAllLines(file);
                    Utilities.CommentOutTextLineSuper(lines, "HudHealthBonusPulse", "HealthBG", false);
                    Utilities.CommentOutTextLineSuper(lines, "HudHealthDyingPulse", "HealthBG", false);
                }

                File.WriteAllLines(file, lines);
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_health_style, ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Toggle custom main menu backgrounds.
        /// </summary>
        /// <remarks>TODO: Consider refactoring.</remarks>
        private static bool MainMenuBackground()
        {
            try
            {
                var directory = new DirectoryInfo(string.Format(Properties.Resources.path_console, MainWindow.HudPath,
                    MainWindow.HudSelection));
                var chapterbackgrounds = string.Format(Properties.Resources.file_chapterbackgrounds, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var chapterbackgroundsTemp = chapterbackgrounds.Replace(".txt", ".file");
                var menu = string.Format(Properties.Resources.file_mainmenuoverride, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(menu);
                var start = Utilities.FindIndex(lines, "Background");
                var index1 = Utilities.FindIndex(lines, "image", Utilities.FindIndex(lines, "if_halloween", start));
                var index2 = Utilities.FindIndex(lines, "image", Utilities.FindIndex(lines, "if_christmas", start));

                if (flawhud.Default.toggle_stock_backgrounds)
                {
                    foreach (var file in directory.GetFiles())
                        File.Move(file.FullName, file.FullName.Replace("upward", "off"));
                    if (File.Exists(chapterbackgrounds))
                        File.Move(chapterbackgrounds, chapterbackgroundsTemp);
                }
                else
                {
                    foreach (var file in directory.GetFiles())
                        File.Move(file.FullName, file.FullName.Replace("off", "upward"));
                    if (File.Exists(chapterbackgroundsTemp))
                        File.Move(chapterbackgroundsTemp, chapterbackgrounds);
                }

                lines[index1] = lines[index1].Replace("//", string.Empty);
                lines[index2] = lines[index2].Replace("//", string.Empty);
                File.WriteAllLines(menu, lines);
                if (flawhud.Default.toggle_stock_backgrounds) return true;

                lines[index1] = Utilities.CommentOutTextLine(lines[index1]);
                lines[index2] = Utilities.CommentOutTextLine(lines[index2]);
                File.WriteAllLines(menu, lines);
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_menu_background,
                    ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Toggle main menu class images.
        /// </summary>
        private static bool MainMenuClassImage()
        {
            try
            {
                var file = string.Format(Properties.Resources.file_mainmenuoverride, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(file);
                var start = Utilities.FindIndex(lines, "TFCharacterImage");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\"{(flawhud.Default.toggle_menu_images ? "-80" : "9999")}\"";
                File.WriteAllLines(file, lines);
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_menu_class_image,
                    ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Toggle to Code Pro fonts instead of the default.
        /// </summary>
        private static bool CodeProFonts()
        {
            try
            {
                var file = string.Format(Properties.Resources.file_clientscheme, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(file);
                lines[Utilities.FindIndex(lines, "clientscheme_fonts")] =
                    $"#base \"scheme/{(flawhud.Default.toggle_code_fonts ? "clientscheme_fonts_pro" : "clientscheme_fonts")}.res\"	// Change to fonts_pro.res for Code Pro fonts";
                File.WriteAllLines(file, lines);
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_fonts, ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Lowers the player health and ammo counters.
        /// </summary>
        private static bool LowerPlayerStats()
        {
            try
            {
                var file = string.Format(Properties.Resources.file_hudlayout, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(file);
                var start = Utilities.FindIndex(lines, "HudWeaponAmmo");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\"{(flawhud.Default.toggle_lower_stats ? "r83" : "c93")}\"";
                start = Utilities.FindIndex(lines, "HudMannVsMachineStatus");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_lower_stats ? "-55" : "0")}\"";
                start = Utilities.FindIndex(lines, "CHealthAccountPanel");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_lower_stats ? "r150" : "267")}\"";
                start = Utilities.FindIndex(lines, "CSecondaryTargetID");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_lower_stats ? "325" : "355")}\"";
                start = Utilities.FindIndex(lines, "HudMenuSpyDisguise");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\"{(flawhud.Default.toggle_lower_stats ? "c60" : "c130")}\"";
                start = Utilities.FindIndex(lines, "HudSpellMenu");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\t\"{(flawhud.Default.toggle_lower_stats ? "c-270" : "c-210")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_huddamageaccount, MainWindow.HudPath,
                    MainWindow.HudSelection);
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "\"DamageAccountValue\"");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_lower_stats ? "r105" : "0")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_playerhealth, MainWindow.HudPath,
                    MainWindow.HudSelection);
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "HudPlayerHealth");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\"{(flawhud.Default.toggle_lower_stats ? "r108" : "c68")}\"";
                File.WriteAllLines(file, lines);

                SetItemEffectPosition(string.Format(Properties.Resources.file_itemeffectmeter, MainWindow.HudPath,
                    MainWindow.HudSelection,
                    string.Empty));
                SetItemEffectPosition(
                    string.Format(Properties.Resources.file_itemeffectmeter, MainWindow.HudPath,
                        MainWindow.HudSelection, "_cleaver"),
                    Positions.Middle);
                SetItemEffectPosition(
                    string.Format(Properties.Resources.file_itemeffectmeter, MainWindow.HudPath,
                        MainWindow.HudSelection, "_sodapopper"),
                    Positions.Top);
                SetItemEffectPosition(
                    string.Format(Properties.Resources.path_resource_ui, MainWindow.HudPath, MainWindow.HudSelection) +
                    "\\huddemomancharge.res",
                    Positions.Middle,
                    "ChargeMeter");
                SetItemEffectPosition(
                    string.Format(Properties.Resources.path_resource_ui, MainWindow.HudPath, MainWindow.HudSelection) +
                    "\\huddemomanpipes.res",
                    Positions.Default,
                    "PipesPresentPanel");
                SetItemEffectPosition(
                    string.Format(Properties.Resources.path_resource_ui, MainWindow.HudPath, MainWindow.HudSelection) +
                    "\\hudrocketpack.res",
                    Positions.Middle);
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_lower_stats, ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     Repositions the player health and ammo counters.
        /// </summary>
        private static bool AlternatePlayerStats()
        {
            try
            {
                // Skip if the player already has "Lowered Player Stats" enabled.
                if (flawhud.Default.toggle_lower_stats) return true;

                var file = string.Format(Properties.Resources.file_hudlayout, MainWindow.HudPath,
                    MainWindow.HudSelection);
                var lines = File.ReadAllLines(file);
                var start = Utilities.FindIndex(lines, "HudWeaponAmmo");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r110" : "c90")}\"";
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r50" : "c93")}\"";
                start = Utilities.FindIndex(lines, "HudMedicCharge");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "c60" : "c38")}\"";
                start = Utilities.FindIndex(lines, "CHealthAccountPanel");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "113" : "c-180")}\"";
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r90" : "267")}\"";
                start = Utilities.FindIndex(lines, "DisguiseStatus");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "115" : "100")}\"";
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r62" : "r38")}\"";
                start = Utilities.FindIndex(lines, "CMainTargetID");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r200" : "265")}\"";
                start = Utilities.FindIndex(lines, "HudAchievementTracker");
                lines[Utilities.FindIndex(lines, "NormalY", start)] =
                    $"\t\t\"NormalY\"\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "135" : "335")}\"";
                lines[Utilities.FindIndex(lines, "EngineerY", start)] =
                    $"\t\t\"EngineerY\"\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "9999" : "335")}\"";
                lines[Utilities.FindIndex(lines, "tall", start)] =
                    $"\t\t\"tall\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "9999" : "95")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_playerhealth, MainWindow.HudPath,
                    MainWindow.HudSelection);
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "HudPlayerHealth");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "137" : "c-120")}\"";
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r47" : "c70")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_huddamageaccount, MainWindow.HudPath,
                    MainWindow.HudSelection);
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "\"DamageAccountValue\"");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "137" : "c-120")}\"";
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r47" : "c70")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_playerhealth, MainWindow.HudPath,
                    MainWindow.HudSelection);
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "HudPlayerHealth");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "10" : "c-190")}\"";
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r75" : "c68")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_playerclass, MainWindow.HudPath,
                    MainWindow.HudSelection);
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "PlayerStatusClassImage");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r125" : "r75")}\"";
                start = Utilities.FindIndex(lines, "classmodelpanel");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r230" : "r200")}\"";
                lines[Utilities.FindIndex(lines, "tall", start)] =
                    $"\t\t\"tall\"\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "180" : "200")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_itemeffectmeter, MainWindow.HudPath,
                    MainWindow.HudSelection, string.Empty);
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "HudItemEffectMeter");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r110" : "c-60")}\"";
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r65" : "c120")}\"";
                start = Utilities.FindIndex(lines, "ItemEffectMeterLabel");
                lines[Utilities.FindIndex(lines, "wide", start)] =
                    $"\t\t\"wide\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "100" : "120")}\"";
                start = Utilities.FindIndex(lines, "\"ItemEffectMeter\"");
                lines[Utilities.FindIndex(lines, "wide", start)] =
                    $"\t\t\"wide\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "100" : "120")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_itemeffectmeter, MainWindow.HudPath,
                    MainWindow.HudSelection, "_cleaver");
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "HudItemEffectMeter");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r85" : "c110")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_itemeffectmeter, MainWindow.HudPath,
                    MainWindow.HudSelection, "_sodapopper");
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "HudItemEffectMeter");
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r75" : "c100")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_itemeffectmeter, MainWindow.HudPath,
                    MainWindow.HudSelection, "_killstreak");
                lines = File.ReadAllLines(file);
                start = Utilities.FindIndex(lines, "HudItemEffectMeter");
                lines[Utilities.FindIndex(lines, "xpos", start)] =
                    $"\t\t\"xpos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "115" : "2")}\"";
                lines[Utilities.FindIndex(lines, "ypos", start)] =
                    $"\t\t\"ypos\"\t\t\t\t\"{(flawhud.Default.toggle_alt_stats ? "r33" : "r28")}\"";
                File.WriteAllLines(file, lines);

                file = string.Format(Properties.Resources.file_hudanimations, MainWindow.HudPath,
                    MainWindow.HudSelection);
                File.WriteAllText(file,
                    flawhud.Default.toggle_alt_stats
                        ? File.ReadAllText(file).Replace("Blank", "HudBlack")
                        : File.ReadAllText(file).Replace("HudBlack", "Blank"));
                return true;
            }
            catch (Exception ex)
            {
                MainWindow.ShowMessageBox(MessageBoxImage.Error, Properties.Resources.error_lower_stats,
                    ex.Message); // TODO: Use a unique error message.
                return false;
            }
        }

        #endregion CONTROLLER
    }
}