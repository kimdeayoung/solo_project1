2026.04.13 Readme 작성 기준
기존 기능 대거 제거

2026.04.05 Readme 작성 기준
Addressables 시스템 구현
Downloader, Loader 시스템이 구현되어 있으며, 로컬 위주의 테스트가 메인(DownLoader의 경우 기존 S3로 테스트된 기능 이후로 변경 X)

BaseAction 시스템 구현
하위 기능으로 Projectile, ApplyStatusInfluence 등 하위 기능 개발 구현 예정
Action 내에 ActionParameter들을 Action에따라 다양하게 사용 가능하게 구현하며, ActionData이후 ActionData 발생하도록 연결구조

JoyStick과 유사한 UI로 플레이어 이동 및 Cam Player Follow 기능 개발