<!DOCTYPE html>
<!--[if IE]><![endif]-->
<html>

  <head>
    <meta charset="utf-8">
      <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
      <title>Class DialogueHUD
 | GPT JRPG documentation </title>
      <meta name="viewport" content="width=device-width">
      <meta name="title" content="Class DialogueHUD
 | GPT JRPG documentation ">
    
      <link rel="shortcut icon" href="../favicon.ico">
      <link rel="stylesheet" href="../styles/docfx.vendor.css">
      <link rel="stylesheet" href="../styles/docfx.css">
      <link rel="stylesheet" href="../styles/main.css">
      <meta property="docfx:navrel" content="../toc.html">
      <meta property="docfx:tocrel" content="toc.html">
    
    <meta property="docfx:rel" content="../">
    
  </head>
  <body data-spy="scroll" data-target="#affix" data-offset="120">
    <div id="wrapper">
      <header>

        <nav id="autocollapse" class="navbar navbar-inverse ng-scope" role="navigation">
          <div class="container">
            <div class="navbar-header">
              <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#navbar">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
              </button>

              <a class="navbar-brand" href="../index.html">
                <img id="logo" class="svg" src="../logo.svg" alt="">
              </a>
            </div>
            <div class="collapse navbar-collapse" id="navbar">
              <form class="navbar-form navbar-right" role="search" id="search">
                <div class="form-group">
                  <input type="text" class="form-control" id="search-query" placeholder="Search" autocomplete="off">
                </div>
              </form>
            </div>
          </div>
        </nav>

        <div class="subnav navbar navbar-default">
          <div class="container hide-when-search" id="breadcrumb">
            <ul class="breadcrumb">
              <li></li>
            </ul>
          </div>
        </div>
      </header>
      <div class="container body-content">

        <div id="search-results">
          <div class="search-list">Search Results for <span></span></div>
          <div class="sr-items">
            <p><i class="glyphicon glyphicon-refresh index-loading"></i></p>
          </div>
          <ul id="pagination" data-first="First" data-prev="Previous" data-next="Next" data-last="Last"></ul>
        </div>
      </div>
      <div role="main" class="container body-content hide-when-search">

        <div class="sidenav hide-when-search">
          <a class="btn toc-toggle collapse" data-toggle="collapse" href="#sidetoggle" aria-expanded="false" aria-controls="sidetoggle">Show / Hide Table of Contents</a>
          <div class="sidetoggle collapse" id="sidetoggle">
            <div id="sidetoc"></div>
          </div>
        </div>
        <div class="article row grid-right">
          <div class="col-md-10">
            <article class="content wrap" id="_content" data-uid="Global.DialogueHUD">


  <h1 id="Global_DialogueHUD" data-uid="Global.DialogueHUD" class="text-break">Class DialogueHUD
</h1>
  <div class="markdown level0 summary"><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="2">Script that controls the Dialogue Window UI element.</p>
</div>
  <div class="markdown level0 conceptual"></div>
  <div class="inheritance">
    <h5>Inheritance</h5>
    <div class="level0"><span class="xref">object</span></div>
    <div class="level1"><span class="xref">UnityEngine.Object</span></div>
    <div class="level2"><span class="xref">UnityEngine.Component</span></div>
    <div class="level3"><span class="xref">UnityEngine.Behaviour</span></div>
    <div class="level4"><span class="xref">UnityEngine.MonoBehaviour</span></div>
    <div class="level5"><span class="xref">DialogueHUD</span></div>
  </div>
  <h6><strong>Namespace</strong>: <a class="xref" href="Global.html">Global</a></h6>
  <h6><strong>Assembly</strong>: Assembly-CSharp.dll</h6>
  <h5 id="Global_DialogueHUD_syntax">Syntax</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">public class DialogueHUD : MonoBehaviour</code></pre>
  </div>
  <h3 id="fields">Fields
</h3>
  <span class="small pull-right mobile-hide">
    <span class="divider">|</span>
    <a href="https://github.com/AIChubar/GPTJRPG/new/master/apiSpec/new?filename=Global_DialogueHUD_chosenOption.md&amp;value=---%0Auid%3A%20Global.DialogueHUD.chosenOption%0Asummary%3A%20&#39;*You%20can%20override%20summary%20for%20the%20API%20here%20using%20*MARKDOWN*%20syntax&#39;%0A---%0A%0A*Please%20type%20below%20more%20information%20about%20this%20API%3A*%0A%0A">Improve this Doc</a>
  </span>
  <span class="small pull-right mobile-hide">
    <a href="https://github.com/AIChubar/GPTJRPG/blob/master/Assets/Scripts/UI/DialogueHUD.cs/#L52">View Source</a>
  </span>
  <h4 id="Global_DialogueHUD_chosenOption" data-uid="Global.DialogueHUD.chosenOption">chosenOption</h4>
  <div class="markdown level1 summary"><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="2">Last chosen <span class="xref">JSONReader.PlayerCharacterAnswer</span>.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="declaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">[HideInInspector]
public JSONReader.PlayerCharacterAnswer chosenOption</code></pre>
  </div>
  <h5 class="fieldValue">Field Value</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><a class="xref" href="Global.JSONReader.html">JSONReader</a>.<a class="xref" href="Global.JSONReader.PlayerCharacterAnswer.html">PlayerCharacterAnswer</a></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <span class="small pull-right mobile-hide">
    <span class="divider">|</span>
    <a href="https://github.com/AIChubar/GPTJRPG/new/master/apiSpec/new?filename=Global_DialogueHUD_content.md&amp;value=---%0Auid%3A%20Global.DialogueHUD.content%0Asummary%3A%20&#39;*You%20can%20override%20summary%20for%20the%20API%20here%20using%20*MARKDOWN*%20syntax&#39;%0A---%0A%0A*Please%20type%20below%20more%20information%20about%20this%20API%3A*%0A%0A">Improve this Doc</a>
  </span>
  <span class="small pull-right mobile-hide">
    <a href="https://github.com/AIChubar/GPTJRPG/blob/master/Assets/Scripts/UI/DialogueHUD.cs/#L73">View Source</a>
  </span>
  <h4 id="Global_DialogueHUD_content" data-uid="Global.DialogueHUD.content">content</h4>
  <div class="markdown level1 summary"><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="2">ScrollView content object.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="declaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">[Tooltip(&quot;ScrollView content object.&quot;)]
public RectTransform content</code></pre>
  </div>
  <h5 class="fieldValue">Field Value</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><span class="xref">UnityEngine.RectTransform</span></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <span class="small pull-right mobile-hide">
    <span class="divider">|</span>
    <a href="https://github.com/AIChubar/GPTJRPG/new/master/apiSpec/new?filename=Global_DialogueHUD_dialogueButtonPrefab.md&amp;value=---%0Auid%3A%20Global.DialogueHUD.dialogueButtonPrefab%0Asummary%3A%20&#39;*You%20can%20override%20summary%20for%20the%20API%20here%20using%20*MARKDOWN*%20syntax&#39;%0A---%0A%0A*Please%20type%20below%20more%20information%20about%20this%20API%3A*%0A%0A">Improve this Doc</a>
  </span>
  <span class="small pull-right mobile-hide">
    <a href="https://github.com/AIChubar/GPTJRPG/blob/master/Assets/Scripts/UI/DialogueHUD.cs/#L68">View Source</a>
  </span>
  <h4 id="Global_DialogueHUD_dialogueButtonPrefab" data-uid="Global.DialogueHUD.dialogueButtonPrefab">dialogueButtonPrefab</h4>
  <div class="markdown level1 summary"><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="2">Prefab of the Button UI representing <span class="xref">JSONReader.PlayerCharacterAnswer.playerCharacterAnswers</span>.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="declaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">[Tooltip(&quot;Prefab of the Button UI representing JSONReader.PlayerCharacterAnswer.playerCharacterAnswers.&quot;)]
public GameObject dialogueButtonPrefab</code></pre>
  </div>
  <h5 class="fieldValue">Field Value</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><span class="xref">UnityEngine.GameObject</span></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <span class="small pull-right mobile-hide">
    <span class="divider">|</span>
    <a href="https://github.com/AIChubar/GPTJRPG/new/master/apiSpec/new?filename=Global_DialogueHUD_enemyDialoguePrefab.md&amp;value=---%0Auid%3A%20Global.DialogueHUD.enemyDialoguePrefab%0Asummary%3A%20&#39;*You%20can%20override%20summary%20for%20the%20API%20here%20using%20*MARKDOWN*%20syntax&#39;%0A---%0A%0A*Please%20type%20below%20more%20information%20about%20this%20API%3A*%0A%0A">Improve this Doc</a>
  </span>
  <span class="small pull-right mobile-hide">
    <a href="https://github.com/AIChubar/GPTJRPG/blob/master/Assets/Scripts/UI/DialogueHUD.cs/#L63">View Source</a>
  </span>
  <h4 id="Global_DialogueHUD_enemyDialoguePrefab" data-uid="Global.DialogueHUD.enemyDialoguePrefab">enemyDialoguePrefab</h4>
  <div class="markdown level1 summary"><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="2">Prefab of the simple UI object containing text of the enemy dialogue phrase.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="declaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">[Tooltip(&quot;Prefab of the simple UI object containing text of the enemy dialogue phrase.&quot;)]
public GameObject enemyDialoguePrefab</code></pre>
  </div>
  <h5 class="fieldValue">Field Value</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><span class="xref">UnityEngine.GameObject</span></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <span class="small pull-right mobile-hide">
    <span class="divider">|</span>
    <a href="https://github.com/AIChubar/GPTJRPG/new/master/apiSpec/new?filename=Global_DialogueHUD_group.md&amp;value=---%0Auid%3A%20Global.DialogueHUD.group%0Asummary%3A%20&#39;*You%20can%20override%20summary%20for%20the%20API%20here%20using%20*MARKDOWN*%20syntax&#39;%0A---%0A%0A*Please%20type%20below%20more%20information%20about%20this%20API%3A*%0A%0A">Improve this Doc</a>
  </span>
  <span class="small pull-right mobile-hide">
    <a href="https://github.com/AIChubar/GPTJRPG/blob/master/Assets/Scripts/UI/DialogueHUD.cs/#L34">View Source</a>
  </span>
  <h4 id="Global_DialogueHUD_group" data-uid="Global.DialogueHUD.group">group</h4>
  <div class="markdown level1 summary"><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="2"><span class="xref">JSONReader.UnitGroup</span> of the current <span class="xref">WorldEnemy</span>.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="declaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">[HideInInspector]
[Tooltip(&quot;JSONReader.UnitGroup of the current WorldEnemy&quot;)]
public JSONReader.UnitGroup group</code></pre>
  </div>
  <h5 class="fieldValue">Field Value</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><a class="xref" href="Global.JSONReader.html">JSONReader</a>.<a class="xref" href="Global.JSONReader.UnitGroup.html">UnitGroup</a></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <span class="small pull-right mobile-hide">
    <span class="divider">|</span>
    <a href="https://github.com/AIChubar/GPTJRPG/new/master/apiSpec/new?filename=Global_DialogueHUD_playerDialoguePrefab.md&amp;value=---%0Auid%3A%20Global.DialogueHUD.playerDialoguePrefab%0Asummary%3A%20&#39;*You%20can%20override%20summary%20for%20the%20API%20here%20using%20*MARKDOWN*%20syntax&#39;%0A---%0A%0A*Please%20type%20below%20more%20information%20about%20this%20API%3A*%0A%0A">Improve this Doc</a>
  </span>
  <span class="small pull-right mobile-hide">
    <a href="https://github.com/AIChubar/GPTJRPG/blob/master/Assets/Scripts/UI/DialogueHUD.cs/#L58">View Source</a>
  </span>
  <h4 id="Global_DialogueHUD_playerDialoguePrefab" data-uid="Global.DialogueHUD.playerDialoguePrefab">playerDialoguePrefab</h4>
  <div class="markdown level1 summary"><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="2">Prefab of the simple UI object containing text of the player dialogue phrase.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="declaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">[Tooltip(&quot;Prefab of the simple UI object containing text of the player dialogue phrase.&quot;)]
public GameObject playerDialoguePrefab</code></pre>
  </div>
  <h5 class="fieldValue">Field Value</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><span class="xref">UnityEngine.GameObject</span></td>
        <td></td>
      </tr>
    </tbody>
  </table>
  <h3 id="methods">Methods
</h3>
  <span class="small pull-right mobile-hide">
    <span class="divider">|</span>
    <a href="https://github.com/AIChubar/GPTJRPG/new/master/apiSpec/new?filename=Global_DialogueHUD_SetDialogueHUD_JSONReader_DialogueInfo_JSONReader_UnitGroup_.md&amp;value=---%0Auid%3A%20Global.DialogueHUD.SetDialogueHUD(JSONReader.DialogueInfo%2CJSONReader.UnitGroup)%0Asummary%3A%20&#39;*You%20can%20override%20summary%20for%20the%20API%20here%20using%20*MARKDOWN*%20syntax&#39;%0A---%0A%0A*Please%20type%20below%20more%20information%20about%20this%20API%3A*%0A%0A">Improve this Doc</a>
  </span>
  <span class="small pull-right mobile-hide">
    <a href="https://github.com/AIChubar/GPTJRPG/blob/master/Assets/Scripts/UI/DialogueHUD.cs/#L80">View Source</a>
  </span>
  <a id="Global_DialogueHUD_SetDialogueHUD_" data-uid="Global.DialogueHUD.SetDialogueHUD*"></a>
  <h4 id="Global_DialogueHUD_SetDialogueHUD_JSONReader_DialogueInfo_JSONReader_UnitGroup_" data-uid="Global.DialogueHUD.SetDialogueHUD(JSONReader.DialogueInfo,JSONReader.UnitGroup)">SetDialogueHUD(DialogueInfo, UnitGroup)</h4>
  <div class="markdown level1 summary"><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="2">Set up the Dialogue UI window.</p>
</div>
  <div class="markdown level1 conceptual"></div>
  <h5 class="declaration">Declaration</h5>
  <div class="codewrapper">
    <pre><code class="lang-csharp hljs">public void SetDialogueHUD(JSONReader.DialogueInfo di, JSONReader.UnitGroup g)</code></pre>
  </div>
  <h5 class="parameters">Parameters</h5>
  <table class="table table-bordered table-striped table-condensed">
    <thead>
      <tr>
        <th>Type</th>
        <th>Name</th>
        <th>Description</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td><a class="xref" href="Global.JSONReader.html">JSONReader</a>.<a class="xref" href="Global.JSONReader.DialogueInfo.html">DialogueInfo</a></td>
        <td><span class="parametername">di</span></td>
        <td><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="1">Current <span class="xref">WorldEnemy</span> dialogue data.</p>
</td>
      </tr>
      <tr>
        <td><a class="xref" href="Global.JSONReader.html">JSONReader</a>.<a class="xref" href="Global.JSONReader.UnitGroup.html">UnitGroup</a></td>
        <td><span class="parametername">g</span></td>
        <td><p sourcefile="api/Global.DialogueHUD.yml" sourcestartlinenumber="1">Current <span class="xref">WorldEnemy</span> unit group.</p>
</td>
      </tr>
    </tbody>
  </table>
  <h3 id="extensionmethods">Extension Methods</h3>
  <div>
      <a class="xref" href="Global.ReflectionExtensions.html#Global_ReflectionExtensions_ToStringWithQuotes_System_Object_">ReflectionExtensions.ToStringWithQuotes(object)</a>
  </div>
</article>
          </div>

          <div class="hidden-sm col-md-2" role="complementary">
            <div class="sideaffix">
              <div class="contribution">
                <ul class="nav">
                  <li>
                    <a href="https://github.com/AIChubar/GPTJRPG/new/master/apiSpec/new?filename=Global_DialogueHUD.md&amp;value=---%0Auid%3A%20Global.DialogueHUD%0Asummary%3A%20&#39;*You%20can%20override%20summary%20for%20the%20API%20here%20using%20*MARKDOWN*%20syntax&#39;%0A---%0A%0A*Please%20type%20below%20more%20information%20about%20this%20API%3A*%0A%0A" class="contribution-link">Improve this Doc</a>
                  </li>
                  <li>
                    <a href="https://github.com/AIChubar/GPTJRPG/blob/master/Assets/Scripts/UI/DialogueHUD.cs/#L10" class="contribution-link">View Source</a>
                  </li>
                </ul>
              </div>
              <nav class="bs-docs-sidebar hidden-print hidden-xs hidden-sm affix" id="affix">
                <h5>In This Article</h5>
                <div></div>
              </nav>
            </div>
          </div>
        </div>
      </div>

      <footer>
        <div class="grad-bottom"></div>
        <div class="footer">
          <div class="container">
            <span class="pull-right">
              <a href="#top">Back to top</a>
            </span>
      GPT JRPG documentation
      
          </div>
        </div>
      </footer>
    </div>

    <script type="text/javascript" src="../styles/docfx.vendor.js"></script>
    <script type="text/javascript" src="../styles/docfx.js"></script>
    <script type="text/javascript" src="../styles/main.js"></script>
  </body>
</html>