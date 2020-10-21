import { Pipe, PipeTransform } from '@angular/core';
import { Status } from '../Enums/status';


@Pipe({ name: 'msfaStatus' })
export class StatusPipe implements PipeTransform {
    transform(value: number): string {
        return Number(value) ? Status[value] : undefined;
    }
}