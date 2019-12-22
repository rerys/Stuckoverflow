import { Component, OnInit, ViewChild, AfterViewInit, ElementRef, OnDestroy } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MAT_DIALOG_DATA, MatDialogRef, MatSnackBar, PageEvent, MatSortHeader } from '@angular/material';
import * as _ from 'lodash';
import { EditUserComponent } from '../edit-user/edit-user.component';
import { StateService } from 'src/app/services/state.service';
import { MatTableState } from 'src/app/helpers/mattable.state';
import { Tag } from 'src/app/models/tag';
import { TagService } from 'src/app/services/tag.service';
import { EditTagComponent } from '../edit-tag/edit-tag.component';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Role } from 'src/app/models/user';

@Component({
    selector: 'app-tagslist',
    templateUrl: './tagslist.component.html',
    styleUrls: ['./tagslist.component.css']
})

export class TagsListComponent implements AfterViewInit, OnDestroy {

    displayedColumns: string[] = ['name','posts', 'actions'];
    dataSource: MatTableDataSource<Tag> = new MatTableDataSource();
    filter: string;
    state: MatTableState;

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: false }) sort: MatSort;

    constructor(
        private tagService: TagService,
        private stateService: StateService,
        public dialog: MatDialog,
        public snackBar: MatSnackBar,
        private authenticationService: AuthenticationService
    ) {
        this.state = this.stateService.tagListState;
    }

    ngAfterViewInit(): void {

        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.dataSource.filterPredicate = (data: Tag, filter: string) => {
            const str = data.name;
            return str.toLowerCase().includes(filter);
        };

        this.state.bind(this.dataSource);
        this.refresh();
    }

    refresh() {

        this.tagService.getAll().subscribe(tags => {
            this.dataSource.data = tags;
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

    edit(tag: Tag) {

        const dlg = this.dialog.open(EditTagComponent, { data: { tag, isNew: false } });
        dlg.beforeClose().subscribe(res => {
            if (res) {
                _.assign(tag, res);
                this.tagService.update(res).subscribe(res => {
                    if (!res) {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.refresh();
                    }
                });
            }
        });
    }

    delete(tag: Tag) {
        const backup = this.dataSource.data;
        this.dataSource.data = _.filter(this.dataSource.data, m => m.name !== tag.name);
        const snackBarRef = this.snackBar.open(`Tag '${tag.name}' will be deleted`, 'Undo', { duration: 10000 });

        snackBarRef.afterDismissed().subscribe(res => {
            if (!res.dismissedByAction)
                this.tagService.delete(tag).subscribe();
            else
                this.dataSource.data = backup;
        });
    }

    create() {
        const tag = new Tag({});
        const dlg = this.dialog.open(EditTagComponent, { data: { tag, isNew: true } });

        dlg.beforeClose().subscribe(res => {
            if (res) {
                this.dataSource.data = [...this.dataSource.data, new Tag(res)];
                this.tagService.add(res).subscribe(res => {
                    if (!res) {
                        this.snackBar.open(`There was an error at the server. The tag has not been created! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.refresh();
                    }
                });
            }
        });
    }

    ngOnDestroy(): void {
        this.snackBar.dismiss();
    }

    canActivate(){
        if(this.authenticationService.currentUser == null){
            return false;
        }else{
            return this.authenticationService.currentUser.role == Role.Admin;
        }
    }

}

