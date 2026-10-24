# KSA Babel 封面 v2：实机风格再创作

- 输出：[ksa-babel-cover-v2.png](ksa-babel-cover-v2.png)。
- 制作日期：2026-10-24。
- 制作方式：内置图像生成工具，以三张实际游戏截图作为视觉输入重新构图；并非未经修改的游戏截图或直接导出的游戏渲染。
- 风格依据：游戏猫咪的巨大球形眼睛、短毛与窄口鼻；截图中的白色多助推器航天器、黑白格纹、暗红喷口、蓝色行星大气，以及深色工业仪表面板。
- 调整：封面主体改用实机猫咪比例；航天器与地平线参考实际飞行画面；标题与四个语言标记采用仪表铭牌风格。
- 原 v1 保留，供对照。本次没有改动游戏安装文件，也没有操作游戏窗口。

## 参考素材

1. [hud-en.png](../../reports/screenshots/hud-en.png)：实际飞行画面与左下角猫咪乘员头像；历史 5602 截图。
2. [full-014-engine-en.png](../../reports/screenshots/full-014-engine-en.png)：较高分辨率实际飞行画面，参考航天器结构、材质和乘员头像。
3. 独立验证档案中的 `ksa_20261024_151332_1920x1080.png`：5638 实机乘员头像及 UI。该参考图保留在本机忽略目录 `work/`，不随封面复制进项目公开素材目录。

前两张参考图的公开来源与固定提交见 [inherited-screenshot-provenance.json](../../reports/inherited-screenshot-provenance.json)。游戏角色、载具及截图中的原有美术元素权利归原权利人；项目代码的 MIT 许可不重新授权这些游戏美术元素。此图作为社区模组封面再创作记录。

## 完整生成提示词

```text
Use case: style-transfer / compositing.
Asset type: revised square mod cover for "KSA Babel", Chinese name "喵语塔".
The user wants the cover to look unmistakably like the actual Kitten Space Agency GAME, based on these genuine gameplay screenshots, rather than a generic cute space-cat illustration. Recreate the attached screenshots as a polished 1:1 community mod cover.
Input references: Images 1 and 2 are real KSA gameplay showing the actual multi-booster spacecraft, planet atmosphere and clouds, hard sunlight, restrained star field, and industrial instrument-panel UI. Images 1, 2 and 3 each have a small live crew portrait near the LOWER LEFT: this specific KSA kitten character anatomy is the most important character reference. Image 3 is from KSA build 5638; its central settings window is NOT part of the desired final cover.
Main subject: rebuild the GAME'S kitten astronaut as a large 3D in-engine-looking bust, using the lower-left crew portraits as the character reference. Preserve the distinctive very large protruding spherical green eyes, broad black pupils and dark eye outlines, relatively small short triangular ears, narrow angular feline muzzle, orange/brown tabby head markings, small pink nose, short close fur and slightly awkward alert expression. The eye spheres must be much larger relative to the face than those of a real kitten. The character should resemble the KSA asset rendered at higher resolution, not an animal photograph or a plush kitten. Use an off-white practical pressure suit, metal helmet collar and clear visor in the same utilitarian game-asset rendering style. Proportions, silhouette and material treatment should take precedence over prettiness.
Environment: use the actual white clustered multi-booster rocket with black checker markings and dark reddish engine bells from Images 1 and 2, recognizable but subordinate in the midground; below it the muted blue planet and scattered cloud fields with a thin blue atmospheric limb. Deep mostly black space, small restrained stars, hard directional sunlight and believable game-rendered PBR surfaces. Strong likeness to these screenshots' restrained, functional visual language.
Graphic design: compose as real game art with a dark charcoal instrument-panel plaque across the lower quarter. Matte dark surfaces, small corner fasteners, very restrained amber and cool green indicators, crisp pale lettering inspired by the game's instrument labels. A thin orbital/communications line and four compact UI-style language tiles displaying only "A", "文", "あ", "Я" convey the multilingual mod; they should look like simple controls, not huge neon fantasy holograms. No invented gigantic tower or space station. All screenshots' original HUD panels, user names, version labels, menus and debug text must be removed.
Text, verbatim: large "KSA BABEL" and smaller "喵语塔" on the bottom instrument plaque. Ensure exact spelling and correctly formed Chinese characters. No other words or numbers. Keep generous 8 percent safe margins around important title and face.
Style constraints: game-asset-inspired 3D render and understated industrial sci-fi graphic design. NO photorealistic natural kitten, NO long silky fluffy fur, NO Disney/Pixar or plush mascot face, NO generic cinematic blue-orange painted space fantasy, NO oversaturated nebula, NO glossy golden advertising typography. No official logos, no watermark, no version number. This is newly composed mod cover art using the supplied gameplay as reference, not a claim to be an unmodified screenshot.
```
