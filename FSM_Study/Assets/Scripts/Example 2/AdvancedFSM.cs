using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedFSM : FSM
{
    //FSM�е�����״̬��ɵ��б�
    private List<FSMState> fsmStates;
    //��ǰ״̬�ı��
    private FSMStateID currentStateID;
    public FSMStateID CurrentStateID { get { return currentStateID; } }

    //��ǰ״̬
    private FSMState currentState;
    public FSMState CurrentState { get { return currentState; } }

    public AdvancedFSM()
    {
        fsmStates = new List<FSMState>(); // ��ʼ��
    }


    public void AddFSMState(FSMState fsmState)
    {
        if (fsmState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }
        //����������״̬��ʱ�� �б��ǿص� ��ô���������б�����
        if (fsmStates.Count == 0)
        {
            fsmStates.Add(fsmState);
            currentState = fsmState;
            currentStateID = fsmState.ID;
            return;
        }

        //���Ҫ�����״̬�Ƿ��Ѿ����б��У�����ǣ�������󲢷���
        foreach (FSMState state in fsmStates)
        {
            if (state.ID == fsmState.ID)
            {
                Debug.LogError("FSM ERROR: Trying to addd a state that was already inside the list");
                return;
            }
        }
        //���Ҫ�����״̬�����б���  ��ô���������б�
        fsmStates.Add(fsmState);
    }

    //��״̬�б���ɾ��һ��״̬
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

    //���ݵ�ǰ״̬ �Ͳ����д��ݵ�װ�� ת�Ƶ��µ�״̬
    public void PerformTransition(Transition trans)
    {
        //���ݵ�ǰ��״̬�� ��transΪ������������GetOutputState����
        //ȷ��ת�ƺ���״̬�ı��
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
