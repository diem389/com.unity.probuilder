using UnityEngine;
using UnityEditor;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using ProBuilder2.Interface;
using System.Linq;

namespace ProBuilder2.Actions
{
	public class FlipFaceEdge : pb_MenuAction
	{
		public override pb_IconGroup group { get { return pb_IconGroup.Geometry; } }
		public override Texture2D icon { get { return pb_IconUtility.GetIcon("Face_FlipTri"); } }
		public override pb_TooltipContent tooltip { get { return _tooltip; } }

		static readonly pb_TooltipContent _tooltip = new pb_TooltipContent
		(
			"Flip Face Edge",
			@"Reverses the direction of the middle edge in a quad.  Use this to fix ridges in quads with varied height corners."
		);

		public override bool IsEnabled()
		{
			return 	pb_Editor.instance != null &&
					selection != null &&
					selection.Length > 0 &&
					selection.Any(x => x.SelectedFaceCount > 0);
		}

		public override bool IsHidden()
		{
			return 	pb_Editor.instance == null ||
					pb_Editor.instance.editLevel != EditLevel.Geometry ||
					pb_Editor.instance.selectionMode != SelectMode.Face;
		}

		public override pb_ActionResult DoAction()
		{
			return pb_Menu_Commands.MenuFlipEdges(selection);
		}
	}
}

