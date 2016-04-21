[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1017:MarkAssembliesWithComVisible", 
        Justification = "Not needed for visual studio extensions")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", Target = "WiredTechSolutions.ShelvesetComparer.ShelvesetComparerNavigationLink.#Execute()", 
        Justification = "Needed to catch general exceptions")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "WiredTechSolutions.ShelvesetComparer.SelectShelvesetSection.#Shelvesets", 
        Justification = "Needed to expose collection as properties because of observable pattern.")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "WiredTechSolutions.ShelvesetComparer.SelectShelvesetSection.#ComparisonShelvesets", 
        Justification = "Needed to expose collection as properties because of observable pattern.")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", 
        Scope = "member", Target = "WiredTechSolutions.ShelvesetComparer.ShelvesetComparerViewModel.#Files", 
        Justification = "Needed to expose collection as properties because of observable pattern.")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
        Scope = "member", 
        Target =
            "WiredTechSolutions.ShelvesetComparer.SelectShelvesetTeamExplorerView.#CompareButtons_Click(System.Object,System.Windows.RoutedEventArgs)", 
        Justification = "Needed to catch general exceptions")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", 
        "CA2000:Dispose objects before losing scope", Scope = "member", 
        Target = "WiredTechSolutions.ShelvesetComparer.ShelvesetComparerViewModel.#Instance", 
        Justification =
            "The object under question is the visual studio DTE instance which won't go out of scope before the window")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", 
        Scope = "member", 
        Target = "WiredTechSolutions.ShelvesetComparer.TeamExplorerBase.#RaisePropertyChanged(System.String)", 
        Justification = "Events cannot be used in this instance")]