import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, ElementRef, ViewChild, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent, MatAutocomplete } from '@angular/material/autocomplete';
import { Tag } from 'src/app/models/tag';
import { TagService } from 'src/app/services/tag.service';


@Component({
    selector: 'tags-editor',
    templateUrl: 'chips-tags.html',
    styleUrls: ['chips-tags.css'],
})
export class ChipsTagsComponent {
    visible = true;
    selectable = true;
    removable = true;
    addOnBlur = true;
    separatorKeysCodes: number[] = [ENTER, COMMA];
    tagCtrl = new FormControl();
    allTags: Tag[];

    @Input() data: Tag[];

    @ViewChild('tagInput', { static: false }) tagInput: ElementRef<HTMLInputElement>;
    @ViewChild('auto', { static: false }) matAutocomplete: MatAutocomplete;

    constructor(
        private tagsService: TagService
    ) {

    }

    remove(tag: Tag): void {
        const index = this.data.indexOf(tag);

        if (index >= 0) {
            this.data.splice(index, 1);
        }
    }

    selected(event: MatAutocompleteSelectedEvent): void {
        this.data.push(event.option.value);
        this.tagInput.nativeElement.value = '';
        this.tagCtrl.setValue(null);
    }

    filterChanged(filterValue: string) {
        var value = filterValue.trim().toLowerCase();
            this.tagsService.find(value).subscribe(tags => {
                    this.allTags = tags;      
            });
    }

    showTagOption(tag: Tag){
        return this.data.find(t => t.id == tag.id) == null;
    }

}