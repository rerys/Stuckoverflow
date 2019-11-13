import { Component, OnInit, ViewChild, AfterViewInit, ElementRef, OnDestroy } from '@angular/core';

import { MatPaginator, MatSort, MatTableDataSource, MatDialog, MAT_DIALOG_DATA, MatDialogRef, MatSnackBar, PageEvent, MatSortHeader } from '@angular/material';

import * as _ from 'lodash';

import { User } from '../../models/user';

import { UserService } from '../../services/user.service';

import { EditUserComponent } from '../edit-user/edit-user.component';

import { StateService } from 'src/app/services/state.service';

import { MatTableState } from 'src/app/helpers/mattable.state';

@Component({

    selector: 'app-userlist',

    templateUrl: './userlist.component.html',

    styleUrls: ['./userlist.component.css']

})

export class UserListComponent implements AfterViewInit, OnDestroy {

    displayedColumns: string[] = ['pseudo', 'firstName', 'fullName', 'email', 'birthDate', 'reputation', 'role', 'actions'];

    dataSource: MatTableDataSource<User> = new MatTableDataSource();

    filter: string;

    state: MatTableState;

    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

    @ViewChild(MatSort, { static: false }) sort: MatSort;

    constructor(

        private userService: UserService,

        private stateService: StateService,

        public dialog: MatDialog,

        public snackBar: MatSnackBar

    ) {

        this.state = this.stateService.userListState;

    }

    ngAfterViewInit(): void {

        this.dataSource.paginator = this.paginator;

        this.dataSource.sort = this.sort;

        this.dataSource.filterPredicate = (data: User, filter: string) => {

            const str = data.pseudo + ' ' + data.firstName + ' ' + data.firstName +' ' + data.lastName + ' ' + data.reputation +' ' + data.birthDate + ' '  + data.roleAsString;

            return str.toLowerCase().includes(filter);

        };


        this.state.bind(this.dataSource);

        this.refresh();

    }

    refresh() {

        this.userService.getAll().subscribe(users => {

            this.dataSource.data = users;

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

    edit(user: User) {

        const dlg = this.dialog.open(EditUserComponent, { data: { user, isNew: false } });

        dlg.beforeClose().subscribe(res => {

            if (res) {

                _.assign(user, res);

                this.userService.update(res).subscribe(res => {

                    if (!res) {

                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });

                        this.refresh();

                    }

                });

            }

        });

    }

    // appelée quand on clique sur le bouton "delete" d'un membre

    delete(user: User) {

        const backup = this.dataSource.data;

        this.dataSource.data = _.filter(this.dataSource.data, m => m.pseudo !== user.pseudo);

        const snackBarRef = this.snackBar.open(`Member '${user.pseudo}' will be deleted`, 'Undo', { duration: 10000 });

        snackBarRef.afterDismissed().subscribe(res => {

            if (!res.dismissedByAction)

                this.userService.delete(user).subscribe();

            else

                this.dataSource.data = backup;

        });

    }

    // appelée quand on clique sur le bouton "new member"

    create() {

        const user = new User({});

        const dlg = this.dialog.open(EditUserComponent, { data: { user, isNew: true } });

        dlg.beforeClose().subscribe(res => {

            if (res) {

                this.dataSource.data = [...this.dataSource.data, new User(res)];

                this.userService.add(res).subscribe(res => {

                    if (!res) {

                        this.snackBar.open(`There was an error at the server. The member has not been created! Please try again.`, 'Dismiss', { duration: 10000 });

                        this.refresh();

                    }

                });

            }

        });

    }

    ngOnDestroy(): void {

        this.snackBar.dismiss();

    }

}

