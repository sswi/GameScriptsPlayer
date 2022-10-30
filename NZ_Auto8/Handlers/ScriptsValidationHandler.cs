using NZ_Auto8.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZ_Auto8.Handlers
{

    /// <summary>
    /// 脚本校验
    /// </summary>
  public static class ScriptsValidationHandler
    {

        /// <summary>
        /// 删除不需要的属性值，减少脚本输出长度，和减少解析计算量，啊~~有点冗余，开始没想到这步， 暂时就这样吧，后期再用反射
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public static Step DleteNotNeedProperty(this Step step)
        {
            switch (step.Mode)
            {
                case EventMode.Keyboard:
                    //step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.Mouse:
                    step.Keyboard = null!;
                    // step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.Sleep:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.FindPicture:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    // step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.Jump:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    // step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.FindPictureClick:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    // step.Picture = null!;
                    step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.FindColor:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    // step.Color = null!;
                    step.InputText = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    break;
                case EventMode.Input:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    //step.InputText = null!;

                    break;
                case EventMode.RandomDelay:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    //step.InputText = null!;
                    break;
                case EventMode.KeyboardReverted:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.Shutdown = null!;
                    step.RandomJump = null!;
                    //step.InputText = null!;
                    break;
                case EventMode.KillApp:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.Shutdown = null!;
                    step.RandomDelay = null!;
                    step.RandomJump = null!;
                    // step.KillApp = null!;
                    //step.InputText = null!;
                    break;
                case EventMode.ShutDown:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    step.RandomJump = null!;
                    //step.InputText = null!;
                    break;
                case EventMode.RandomJump:
                    step.Keyboard = null!;
                    step.Mouse = null!;
                    step.Jump = null!;
                    step.Picture = null!;
                    step.Color = null!;
                    step.RandomDelay = null!;
                    step.KillApp = null!;
                    //step.InputText = null!;
                    break;
            }
            return step;
        }


        /// <summary>
        /// 跳转步骤重定向
        /// </summary>
        /// <param name="Scripts"></param>
        /// <returns></returns>
        public static bool RedirectJump(this ObservableCollection<Step> Scripts)
        {
            //将脚本步数转换为列表，好用lamda操作
            var list = Scripts.ToList();

            foreach (var item in Scripts)
            {

                DleteNotNeedProperty(item);
                //Debug.WriteLine($"{item.Index},{item.Mode}");

                //找图跳转
                if (item.Mode == EventMode.FindPicture || item.Mode == EventMode.FindPictureClick)
                {
                    var s = list.FindLast(s => item.Picture.NofFoundTargetTag != null && s.JumTargetTag == item.Picture.NofFoundTargetTag);
                    if (s != null)
                    {
                        item.Picture.NotFoundJumToIndex = s.Index;
                    }
                    else if (item.Picture.NofFoundTargetTag != null && item.Picture.NofFoundTargetTag.Length > 0)
                    {
                     System.Windows. MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，未找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                        return false;
                    }
                    s = list.FindLast(s => !string.IsNullOrEmpty(item.Picture.HasFoundTargetTag) && s.JumTargetTag == item.Picture.HasFoundTargetTag);
                    if (s != null)
                    {
                        item.Picture.HasFoundJumToIndex = s.Index;
                    }
                    else if (!string.IsNullOrEmpty(item.Picture.HasFoundTargetTag))
                    {
                        System.Windows.MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                        return false;
                    }



                }

                //找色跳转
                else if (item.Mode == EventMode.FindColor)
                {
                    var s = list.FindLast(s => item.Color.NofFoundTargetTag != null && s.JumTargetTag == item.Color.NofFoundTargetTag);
                    if (s != null)
                    {
                        item.Color.NotFoundJumToIndex = s.Index;
                    }
                    else if (item.Color.NofFoundTargetTag != null && item.Color.NofFoundTargetTag.Length > 0)
                    {
                        System.Windows.MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，未找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                        return false;
                    }
                    s = list.FindLast(s => item.Color.HasFoundTargetTag != null && s.JumTargetTag == item.Color.HasFoundTargetTag);
                    if (s != null)
                    {
                        item.Color.HasFoundJumToIndex = s.Index;
                    }
                    else if (item.Color.HasFoundTargetTag != null && item.Color.HasFoundTargetTag.Length > 0)
                    {
                        System.Windows.MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，找到跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                        return false;
                    }



                }


                //纯跳转
                else if (item.Mode == EventMode.Jump)
                {

                    var s = list.FindLast(s => item.Jump.TargetTag != null && s.JumTargetTag == item.Jump.TargetTag);
                    if (s != null)
                    {
                        item.Jump.JumToIndex = s.Index;
                    }
                    else if (item.Jump.TargetTag != null && item.Jump.TargetTag.Length > 0)
                    {
                        System.Windows.MessageBox.Show($"脚本初始化失败，第 {item.Index} 步，跳转目标标签： {item.JumTargetTag}  不存在，请重新确认");
                        return false;
                    }
                    item.Jump.CyclesCount = 0;
                }

                //随机跳转
                else if (item.Mode == EventMode.RandomJump)
                {
                    //先将跳转标记转换成跳转步数
                    //判断是否为空或者目标少于2个
                    if (!string.IsNullOrEmpty(item.RandomJump.TargetTags) && item.RandomJump.TargetTags.Contains('|'))
                    {

                        var tags = item.RandomJump.TargetTags.Split('|');
                        item.RandomJump.RandomJumpTargets.Clear();
                        foreach (var tag in tags)
                        {
                            //找出对应标签的序号
                            var tagIndex = list.Find(s => s.JumTargetTag == tag);

                            if (tagIndex != null)
                            {
                                var target = new RandomJumpTarget() { TargetTag = tag, TargetIndex = tagIndex.Index };
                                item.RandomJump.RandomJumpTargets.Add(target);
                            }
                            else
                            {
                                System.Windows.MessageBox.Show($"第{item.Index}步，随机跳转目标标记 {tag} 无法找到");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show($"第{item.Index}步，随机跳转目标少于2个，无法随机跳转");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
