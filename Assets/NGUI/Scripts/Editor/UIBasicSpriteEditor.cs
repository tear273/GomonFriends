//-------------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2020 Tasharen Entertainment Inc
//-------------------------------------------------

using UnityEditor;
using UnityEngine;

/// <summary>
/// Inspector class used to edit UI sprites and textures.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UIBasicSprite), true)]
public class UIBasicSpriteEditor : UIWidgetInspector
{
	/// <summary>
	/// Draw all the custom properties such as sprite type, flip setting, fill direction, etc.
	/// </summary>

	protected override void DrawCustomProperties ()
	{
		GUILayout.Space(6f);

		SerializedProperty sp = NGUIEditorTools.DrawProperty("Type", serializedObject, "mType", GUILayout.MinWidth(20f));

		UISprite.Type type = (UISprite.Type)sp.intValue;

		if (type == UISprite.Type.Simple)
		{
			NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");

			NGUIEditorTools.DrawProperty("Round", serializedObject, "mRound");
			NGUIEditorTools.DrawProperty("LT", serializedObject, "mLTRound");
			NGUIEditorTools.DrawProperty("RT", serializedObject, "mRTRound");
			NGUIEditorTools.DrawProperty("LB", serializedObject, "mLBRound");
			NGUIEditorTools.DrawProperty("RB", serializedObject, "mRBRound");
		}
		else if (type == UISprite.Type.Tiled)
		{
			NGUIEditorTools.DrawBorderProperty("Trim", serializedObject, "mBorder");
			NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");
		}
		else if (type == UISprite.Type.Sliced)
		{
			NGUIEditorTools.DrawBorderProperty("Border", serializedObject, "mBorder");
			NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");

			EditorGUI.BeginDisabledGroup(sp.hasMultipleDifferentValues);
			{
				sp = serializedObject.FindProperty("centerType");
				bool val = (sp.intValue != (int)UISprite.AdvancedType.Invisible);

				if (val != EditorGUILayout.Toggle("Fill Center", val))
				{
					sp.intValue = val ? (int)UISprite.AdvancedType.Invisible : (int)UISprite.AdvancedType.Sliced;
				}
			}
			EditorGUI.EndDisabledGroup();
		}
		else if (type == UISprite.Type.Filled)
		{
			NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");
			NGUIEditorTools.DrawProperty("Fill Dir", serializedObject, "mFillDirection", GUILayout.MinWidth(20f));
			GUILayout.BeginHorizontal();
			GUILayout.Space(4f);
			NGUIEditorTools.DrawProperty("Fill Amount", serializedObject, "mFillAmount", GUILayout.MinWidth(20f));
			GUILayout.Space(4f);
			GUILayout.EndHorizontal();
			NGUIEditorTools.DrawProperty("Invert Fill", serializedObject, "mInvert", GUILayout.MinWidth(20f));
		}
		else if (type == UISprite.Type.Advanced)
		{
			NGUIEditorTools.DrawBorderProperty("Border", serializedObject, "mBorder");
			NGUIEditorTools.DrawProperty("  Left", serializedObject, "leftType");
			NGUIEditorTools.DrawProperty("  Right", serializedObject, "rightType");
			NGUIEditorTools.DrawProperty("  Top", serializedObject, "topType");
			NGUIEditorTools.DrawProperty("  Bottom", serializedObject, "bottomType");
			NGUIEditorTools.DrawProperty("  Center", serializedObject, "centerType");
			NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");
		}
		
		if (type == UIBasicSprite.Type.Simple || type == UIBasicSprite.Type.Sliced) // Gradients get too complicated for tiled and filled.
		{
			GUILayout.BeginVertical();
			SerializedProperty gr = NGUIEditorTools.DrawProperty("Gradient", serializedObject, "mApplyGradient", GUILayout.Width(95f));

			
			


			EditorGUI.BeginDisabledGroup(!gr.hasMultipleDifferentValues && !gr.boolValue);
			{

				

				GUILayout.BeginVertical();
				GUILayout.BeginHorizontal();
				GUILayout.Space(79f);
				NGUIEditorTools.DrawProperty("TopPersent", serializedObject, "mPersentGradientT", GUILayout.Width(200f));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Space(79f);
				NGUIEditorTools.DrawProperty("BottomPersent", serializedObject, "mPersentGradientB", GUILayout.Width(200f));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Space(79f);
				SerializedProperty leftToright = NGUIEditorTools.DrawProperty("LeftToRight", serializedObject, "mApplyLtR", GUILayout.Width(95f));
				GUILayout.EndHorizontal();




				EditorGUI.BeginDisabledGroup(!leftToright.boolValue);
				{
					GUILayout.BeginHorizontal();
					GUILayout.Space(79f);
					NGUIEditorTools.SetLabelWidth(50f);
					serializedObject.DrawProperty("mGradientLeft", "Left", GUILayout.MinWidth(40f));
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					NGUIEditorTools.SetLabelWidth(50f);
					GUILayout.Space(79f);




					serializedObject.DrawProperty("mGradientRight", "Right", GUILayout.MinWidth(40f));
					NGUIEditorTools.SetLabelWidth(80f);
					GUILayout.EndHorizontal();
					GUILayout.EndVertical();
				}
				EditorGUI.EndDisabledGroup();

				//ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

				GUILayout.BeginHorizontal();
				GUILayout.Space(79f);
				SerializedProperty topToBottom = NGUIEditorTools.DrawProperty("TopToBottom", serializedObject, "mApplyTtB", GUILayout.Width(95f));
				GUILayout.EndHorizontal();

				EditorGUI.BeginDisabledGroup(!topToBottom.boolValue);
				{
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();

					GUILayout.Space(79f);
					NGUIEditorTools.SetLabelWidth(50f);
					serializedObject.DrawProperty("mGradientTop", "Top", GUILayout.MinWidth(40f));
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					NGUIEditorTools.SetLabelWidth(50f);
					GUILayout.Space(79f);

					serializedObject.DrawProperty("mGradientBottom", "Bottom", GUILayout.MinWidth(40f));
					NGUIEditorTools.SetLabelWidth(80f);
				}
				EditorGUI.EndDisabledGroup();


			}
			EditorGUI.EndDisabledGroup();
			GUILayout.EndVertical();
		}
		base.DrawCustomProperties();
	}
}
