using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFSM : FSM
{
    //FSM中的所有状态组成的列表
    private List<FSMState> fsmStates;
    //当前状态的编号
    private FSMStateID currentStateID;
    public FSMStateID CurrentStateID { get { return currentStateID; } }

    //当前状态
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public AdvancedFSM()
    {
        fsmStates = new List<FSMState>(); // 初始化
    }


    public void AddFSMState(FSMState fsmState)
    {
        if (fsmState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }
        //如果插入这个状态的时候 列表还是控的 那么将它加入列表并返回
        if (fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.ID;
            return;
        }

        //检查要加入的状态是否已经在列表中，如果是，报告错误并返回
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == fsmState.ID)
            {
                Debug.LogError("FSM ERROR: Trying to addd a state that was already inside the list");
                return;
            }
        }
        //如果要加入的状态不在列表中  那么将它加入列表
        fsmStates.Add(fsmState);
    }

    //从状态列表中删除一个状态
    public void DeleteState(FSMStateID fsmState)
    {
        foreach(FSMState state in fsmStates)
        {
            if(state.ID == fsmState)
            {
                fsmStates.Remove(state);
                return;
            }
        }
        Debug.LogError("FSM ERROR: The state passed was not on the list. Impossible to delete it.");
    }

    //根据当前状态 和参数中传递的装欢 转移到新的状态
    public void PerformTransition(Transition trans)
    {
        //根据当前的状态类 以trans为参数调用它的GetOutputState方法
        //确定转移后新状态的编号
        FSMStateID id = currentState.GetOutputState(trans);
        currentStateID = id;
        foreach(FSMState state in fsmStates)
        {
            if(state.ID == currentStateID)
            {
                currentState = state;
                break;
            }
        }
    }

  

}
