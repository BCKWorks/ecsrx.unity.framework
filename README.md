![IMG](https://img.shields.io/badge/pkg%20name-com.bckworks.ecsrx.unity.framework-yellowgreen?style=for-the-badge&logo=appveyor)

![NPM](https://img.shields.io/npm/v/com.bckworks.ecsrx.unity.framework)
![NPM](https://img.shields.io/npm/l/com.bckworks.ecsrx.unity.framework)

# 소개

BCKWorks EcsRx Unity Framework는 BCKWorks에서 [유니티]를 기반으로 하여 제작된 소프트웨어 개발 도구(SDK)입니다.

> `2021.1.x` 버전에 맞춰서 제작되었습니다.

# 시작하기

## 프로젝트 설정하기

* [유니티]를 통해 비어있는 3D 템플릿으로 새로운 프로젝트를 생성하거나 기존 프로젝트를 불러옵니다.
* 프로젝트의 `Scripting Runtime Version`이 `NET 4.x`으로 설정되어 있는지 확인합니다.
  * 유니티 편집기 상에서 `Edit -> Project Settings`를 통해서 `Project Settings`창을 오픈합니다.
  * `Project Settings`창에서 왼쪽 메뉴를 통해 'Player'를 선택합니다.
  * `Player`설정 패널에서 `Other Settings`를 펼칩니다.
  * `Scripting Runtime Version`이 `.NET 4.x`로 설정되었는지 확인합니다.

## 유니티 프로젝트 매니페스트에 패키지 추가하기

* 프로젝트의 `Packages`디렉토리를 탐색합니다.
* [프로젝트-메니패스트]인 `manifest.json`을 수정하기 위해 텍스트 편집기에서 오픈합니다.
  * `https://registry.npmnjs.org/`가 `scopedRegistries`에 포함되었는지 확인합니다.
    * `com.bckworks`이 `scopes`에 포함되었는지 확인합니다.
  * `dependencies`에 `com.bckworks.ecsrx.unity.framework`의 최신버전을 추가합니다.

 간략한 예제는 다음과 같습니다. 여기에 표기된 `"X.Y.Z"` 버전은 [최신-릴리즈(NPM)]인
 ![NPM](https://img.shields.io/npm/v/com.bckworks.ecsrx.unity.framework)에서 v를 제외한 나머지로 자리에 맞추어 대체 되어야 합니다.
```json
{
  "scopedRegistries": [
    {
      "name": "BCKWorks",
      "url": "https://registry.npmjs.org/",
      "scopes": [
        "com.bckworks"
      ]
    }
  ],
  "dependencies": {
    "com.bckworks.ecsrx.unity.framework":  "X.Y.Z",
    ...
  }
}
```
* 메니페스트 편집을 마치고 저장한 후 유니티로 다시 돌아가면 패키지가 자동으로 추가됩니다.

## 최신 버전으로 업데이트하기

위의 과정을 통해 얻은 패키지는 유니티 패키지 매니저 UI에 나타날 것입니다. 이후로 유니티 패키지 매니저 UI 상에서 업데이트가 가능할 경우 `Update` 버튼이 활성화 되며 이를 클릭할 시 해당 버전으로 자동 업데이트 됩니다.

[유니티]: https://unity3d.com/
[최신-릴리즈(NPM)]: https://www.npmjs.com/package/com.bckworks.ecsrx.unity.framework
[프로젝트-매니페스트]: https://docs.unity3d.com/Manual/upm-manifestPrj.html
