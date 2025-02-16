﻿using System.Collections.Generic;

namespace ColdMint.scripts.camp;

/// <summary>
/// <para>camp</para>
/// <para>阵营</para>
/// </summary>
public class Camp
{
    private readonly List<string> _friendlyCampIdList;

    public Camp(string id)
    {
        Id = id;
        _friendlyCampIdList = new List<string>();
    }

    /// <summary>
    /// <para>Add friendly camp ID</para>
    /// <para>添加友善的阵营ID</para>
    /// </summary>
    /// <param name="friendlyCampId"></param>
    public void AddFriendlyCampId(string friendlyCampId)
    {
        _friendlyCampIdList.Add(friendlyCampId);
    }

    /// <summary>
    /// <para>Get camp ID</para>
    /// <para>获取阵营ID</para>
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// <para>Get camp name</para>
    /// <para>获取阵营名</para>
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// <para>Friend Injury</para>
    /// <para>友伤</para>
    /// </summary>
    /// <remarks>
    ///<para>Whether to damage targets on the same side</para>
    ///<para>是否可伤害同一阵营的目标</para>
    /// </remarks>
    public bool FriendInjury { get; set; }

    /// <summary>
    /// <para>Gets the camp ID that is friendly to this camp</para>
    /// <para>获取与此阵营友好的阵营ID</para>
    /// </summary>
    public string[] GetFriendlyCampIdArray()
    {
        return _friendlyCampIdList.ToArray();
    }
}