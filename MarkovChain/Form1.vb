Public Class Form1

    Public sequence As New Dictionary(Of String, List(Of String))
    Dim sampleSize As Integer

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CreateSequence()
    End Sub

    Sub CreateSequence()
        sequence.Clear()

        txtRefText.Text = txtRefText.Text.Replace(vbCrLf, " ").Replace(vbNewLine, " ").Replace(vbCr, " ").Replace("  ", " ")

        If txtRefText.TextLength = 0 Then
            Exit Sub
        End If

        Dim refText As String = txtRefText.Text
        sampleSize = NumericUpDown1.Value

        ProgressBar1.Maximum = refText.Length
        ProgressBar1.Value = 0
        ProgressBar1.Step = 1

        For i As Integer = 0 To refText.Length - 1 - sampleSize

            Dim sample As String = refText.Substring(i, sampleSize)
            Dim nextChar As String = refText.Substring(i + sampleSize, 1)

            If Not sequence.Where(Function(x) (x.Key = sample)).Any Then
                sequence.Add(sample, updateStringList(Nothing, nextChar))
            Else
                sequence.Item(sample) = updateStringList(sequence.Item(sample), nextChar)
            End If

            ProgressBar1.PerformStep()
            Application.DoEvents()
        Next

        ProgressBar1.Value = 0

    End Sub

    Function updateStringList(l As List(Of String), text As String) As List(Of String)
        If l Is Nothing Then
            l = New List(Of String)
            l.Add(text)
        Else
            If Not l.Contains(text) Then
                l.Add(text)
            End If
        End If

        Return l
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        txtResult.Clear()

        Dim maxChars As Integer = NumericUpDown2.Value

        Dim wordList As List(Of String) = txtRefText.Text.Split(" ").Distinct.ToList

        Dim search As String = wordList(Rnd() * (wordList.Count - 1))

        Do While search.Length < sampleSize OrElse search.First = Char.ToLower(search.First)
            search = wordList(Rnd() * (wordList.Count - 1))
        Loop
        search = search.Substring(0, sampleSize)


        Dim result As String = search

        For i As Integer = 0 To maxChars - 1

            If Not sequence.ContainsKey(search) Then Continue For

            Dim sList As List(Of String) = sequence.Item(search)

            result &= sList.Item(Rnd() * (sList.Count - 1))

            search = result.Substring(result.Length - sampleSize, sampleSize)

        Next

        txtResult.AppendText(result)

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://defnesumanblogs.com/category/turkce-yazilar/")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("https://tr.wikipedia.org/wiki/Markov_zinciri")
    End Sub
End Class
