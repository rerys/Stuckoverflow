import { Component, OnInit, ViewChild, AfterViewInit, ElementRef, OnDestroy } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MAT_DIALOG_DATA, MatDialogRef, MatSnackBar, PageEvent, MatSortHeader } from '@angular/material';
import * as _ from 'lodash';
import { StateService } from 'src/app/services/state.service';
import { MatTableState } from 'src/app/helpers/mattable.state';
import { Post } from 'src/app/models/post';
import { PostService } from 'src/app/services/post.service';

@Component({
    selector: 'app-questionsList',
    templateUrl: './questionsList.component.html',
    styleUrls: ['./questionsList.component.css']
})


export class QuestionsListComponent implements AfterViewInit {

    displayedColumns: string[] = ['title','body', 'timestamp'];
    dataSource: MatTableDataSource<Post> = new MatTableDataSource();
    filter: string;
    state: MatTableState;

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    constructor
        (
            private postService: PostService,
            private stateService: StateService,
            public dialog: MatDialog,
            public snackBar: MatSnackBar

        ) {
        this.state = this.stateService.questionListState;
    }


    ngAfterViewInit(): void {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;

        this.dataSource.filterPredicate = (data: Post, filter: string) => {
            const str = data.title + ' ' + data.body + ' ' + data.timestamp;
            return str.toLowerCase().includes(filter);
        };

        this.state.bind(this.dataSource);
        this.refresh();

    }

    refresh() {

        this.postService.getQuestions().subscribe(q => {
            this.dataSource.data = q;
            this.state.restoreState(this.dataSource);
            this.filter = this.state.filter;
        });
    }

    filterChanged(filterValue: string) {

        this.dataSource.filter = filterValue.trim().toLowerCase();
        this.state.filter = this.dataSource.filter;
        if (this.dataSource.paginator)
            this.dataSource.paginator.firstPage();

    }

    // newest(){
    //     this.postService.getQuestionsByNewest().subscribe(q => {
    //         this.dataSource.data = q;
    //         this.state.restoreState(this.dataSource);
    //         this.filter = this.state.filter;
    //     });
    // }
    

}
