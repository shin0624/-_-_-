using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.PlainButton;
using UnityEngine.UIElements;
using System;
using SceneManagement;
using SaveSystem.Scripts.Runtime;
using SaveSystem.Scripts.Runtime.Channels;
using UnityEngine.Playables;

[RequireComponent(typeof(UIDocument))]

public class main : MonoBehaviour
{
    private UIDocument m_UIDocument;
    private VisualElement m_ConfirmationModal;
    private VisualElement m_Credits;
    [SerializeField] private LoadSceneChannel m_LoadSceneChannel;
    [SerializeField] private SceneReference m_StartingLocation;
    [SerializeField] private GameData m_GameData;
    [SerializeField] private LoadDataChannel m_LoadDataChannel;

    [SerializeField] private PlayableDirector timeline; // ��ŸƮ �� ����� Ÿ�Ӷ��� ����
    [SerializeField] private AudioSource ado;//���� �⺻ �����
    private VisualElement root;


    private void Awake()
    {
      m_UIDocument = GetComponent<UIDocument>();

        if(timeline==null)
        {
            Debug.LogError("PlayableDirector component not assigned!");
            return;
        }

        if(ado==null)
        {
            Debug.LogError("AudioSource component not assigned!");
        }
    }

    private void OnEnable()
    {
        //���� �̾�ϱ�: continue-��ư
        root = m_UIDocument.rootVisualElement;
        PlainButton continueButton = m_UIDocument.rootVisualElement.Q<PlainButton>(name: "button-continue");
        continueButton.SetEnabled(m_GameData.hasPreviousSave);
        continueButton.clicked += ContinuePreviouseGame;


        //------------------------------------------------------------------------------------------------------

        //���� ����: exit-��ư
        PlainButton exitButton = root.Q<PlainButton>("button-exit");
        exitButton.clicked += ShowConfirmationModal;
        //exitButton.clicked += () => Debug.Log("exit test"); ��ư ȣ�� �׽�Ʈ

        //���� �����ư Ŭ���� �ѹ� �� �ǻ縦 ����� �˾�â
        m_ConfirmationModal = root.Q("confirmation-modal");

        //���� �����ϱ�: quit-��ư
        Button confirmation = m_ConfirmationModal.Q<Button>("button-quit");
        confirmation.clicked += QuitGame;

        //���� ���� ���� �ʰ� ����ϱ�: concle-��ư
        Button cancelButton=m_ConfirmationModal.Q<Button>("button-cancle");
        cancelButton.clicked += Cancel;

        //------------------------------------------------------------------------------------------------------

        //new ���� ���� : new-game-��ư
        Button newGameButton = root.Q<PlainButton>("button-new-game");
        newGameButton.clicked += StartNewGame;


        //------------------------------------------------------------------------------------------------------

        //credit menu: credit ��ư

        PlainButton creditButton = root.Q<PlainButton>("button-credits");
        m_Credits = root.Q("credit-modal");
        creditButton.clicked += OpenCredits;

        //credit menu close button: ũ���� ȭ���� �ݱ� ��ư
        Button closeCreditsButton = m_Credits.Q<Button>("button-close");
        closeCreditsButton.clicked += CloseCredits;

        timeline.stopped += OnTimelineFinished;//Ÿ�Ӷ��� ���� �̺�Ʈ �ڵ鷯 �߰�
        ado.Play();
    }

    private void OnDisable()
    {
        timeline.stopped -= OnTimelineFinished;//�̺�Ʈ �ڵ鷯 ����
    }

    private void OpenCredits()
    {
        m_Credits.style.display = DisplayStyle.Flex;

    }

    private void CloseCredits()
    {
        m_Credits.style.display = DisplayStyle.None;

    }

    private void ContinuePreviouseGame()
    {
        m_GameData.LoadFromBinaryFile();
        m_LoadDataChannel.Load();

    }
    private void StartNewGame()
    {
        //LoadingSceneManager.LoadScene("New1_2FloorScene");
        // m_LoadSceneChannel.Load(m_StartingLocation);

       root.style.display= DisplayStyle.None;//��ŸƮ��ư Ŭ�� �� ui��Ȱ��ȭ.
        ado.Stop();
        timeline.Play();//��ŸƮ��ư Ŭ�� �� Ÿ�Ӷ��� ���
    }

    //���� ���� �˾�â ����
    private void ShowConfirmationModal()
    {
        m_ConfirmationModal.style.display = DisplayStyle.Flex;
    }
    //���� ���� �˾�â �����
    private void Cancel()
    {
        m_ConfirmationModal.style.display= DisplayStyle.None;
    }
    //���� �����Ű�� 
    private void  QuitGame()
    { 
        m_ConfirmationModal.style.display=DisplayStyle.None;
        Application.Quit();
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        LoadingSceneManager.LoadScene("New1_2FloorScene");//�ε����� ���� ȣ�� �� �ش� ���� ȣ��.
    }
  

}
