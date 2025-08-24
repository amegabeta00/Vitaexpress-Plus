---
<p align="center"> <img alt="Space Station 14" width="780" height="270" src="https://raw.githubusercontent.com/space-wizards/asset-dump/de329a7898bb716b9d5ba9a0cd07f38e61f1ed05/github-logo.svg" /></p>

Europa — это форк [Goob Station](https://github.com/Goob-Station/Goob-Station), ориентирующийся на идеи [Старо-TG](https://github.com/tgstation/tgstation) и [Shiptest](https://github.com/shiptest-ss13/Shiptest) из Space Station 13, включая в это свои собственные идеи.

---
## Ссылки

<div class="header" align="center">

[Discord](https://discord.gg/mk-europa) | [Steam](https://store.steampowered.com/app/1255460/Space_Station_14/) | [Standalone Download](https://spacestation14.com/about/nightlies/)

</div>

[<img src="https://i.imgur.com/xMzKtYK.png" alt="Discord" width="150" align="left">](https://discord.gg/mk-europa)
**Discord Server**<br>В космосе вас никто не услышит.

[<img src="https://i.imgur.com/XiS9QP5.png" alt="ASF" width="150" align="left">](https://github.com/AtaraxiaSpaceFoundation)
**Ataraxia Space Foundation**<br>Специализируемся на разработке этого билда.

---
## Документация

Джубами имеет [сайт](https://docs.goobstation.com/) с документацией к контенту, билда, движка, дизайна игры и многому другому. Там также достаточно много полезного для новичков в разработке.

---
## Контрибуция

Мы рады любой помощи в разработке. Для этого вы можете перейти на основной сервер [разработки Джубами](https://discord.gg/zXk2cyhzPN) и помочь нам, всем и им - на прямую! Для этого вы можете в любой момент посмотреть на [список проблем](https://github.com/Goob-Station/Goob-Station/issues) которые стоило бы решить и сделать это может каждый. Не стесняйтесь попросить помощи!

---
## Сборка

Следуйте гайду от [Джубами](https://docs.goobstation.com/en/general-development/setup.html) по настройке рабочей среды, но учитывайте, что репозитории отличаются друг от друга и некоторые вещи могут отличаться.
Ниже перечислены скрипты и методы облегчающие работу с билдом.

> [!TIP]
> Используйте [IDE Rider](https://github.com/designinlife/jetbrains), он неимоверно облегчит вам жизнь, если вы собираетесь влиться в разработку (код), или билдить сборку, больше пары раз.


### Windows

> 1. Клонируйте данный репозиторий.
```shell
git clone https://github.com/AtaraxiaSpaceFoundation/Europa-Station-14.git
```
> 2. Откройте коммандную строку в папке репозитория и введите команду для того, чтобы скачать движок игры.
```shell
git submodule update --init --recursive
```
> 3. Следующим этапом идёт билд-билда, для этого нужно ввести команду с указанием того, для чего вы билдите, для этого нужно написать Release, Tools или Debug.
```shell
dotnet build --configuration Release/Tools/Debug
```
> [!TIP]
> К примеру Release - полная версия, Tools - урезаная версия, Debug - урезаная версия, но которая будет вылетать при любой ошибке. В большинстве случаев вам хватит Tools, что-бы не перенапрягать машину.

> 4. Далее вам требуется запустить сервер с клиентом, для этого есть несколько способов.
> - 4.1. Командами, в конце так же можно указать вместо Tools любой интересующий вас тип.
```shell
dotnet run --project Content.Server --configuration Tools
```
```shell
dotnet run --project Content.Client --configuration Tools
```
> - 4.2. Запуск .bat файла, который автоматически выполнит те же команды.
```shell
Scripts/bat/runQuickAll.bat
```
> 5. Подключитесь к localhost в появившемся окне и играйте!

---
## Лицензия

All code in this codebase is released under the AGPL-3.0-or-later license. Each file includes REUSE Specification headers or separate .license files that specify a dual license option. This dual licensing is provided to simplify the process for projects that are not using AGPL, allowing them to adopt the relevant portions of the code under an alternative license. You can review the complete texts of these licenses in the LICENSES/ directory.

Most media assets are licensed under [CC-BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/) unless stated otherwise. Assets have their license and the copyright in the metadata file. [Example](https://github.com/space-wizards/space-station-14/blob/master/Resources/Textures/Objects/Tools/crowbar.rsi/meta.json).

> [!NOTE]
> Some assets are licensed under the non-commercial [CC-BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) or similar non-commercial licenses and will need to be removed if you wish to use this project commercially.
