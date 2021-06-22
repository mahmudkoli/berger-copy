import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { Directive,ElementRef,Input,OnInit,Renderer2} from '@angular/core';

@Directive({
    selector: '[numberFormatColor]',
    providers: [
        CurrencyPipe,
        DecimalPipe
    ]
})

export class NumberFormatColorDirective implements OnInit {
    @Input() numberFormatColor: any;
    @Input() numberFormatColorBg: boolean = false;
    @Input() numberFormatColorFraction: boolean = true;
    element: any;

    constructor(
        private renderer: Renderer2, 
        private elementRef: ElementRef, 
        private currencyPipe: CurrencyPipe, 
        private decimalPipe: DecimalPipe) {
        this.element = this.elementRef.nativeElement;
    }

    ngOnInit() {
        this.updateValueFormat();
    }

    updateValueFormat() {
        const negativeNumberFormatColorStyles = {
            'color': 'red',
        };
        const negativeNumberFormatColorBgStyles = {
            'background-color': 'red',
            'color': 'white',
            'padding': '3px',
            'border-radius': '5px',
        };
        
        let value = this.numberFormatColor || 0;
        // const formatValue = this.decimalPipe.transform(value, '1.0-2', 'en-IN');
        // const formatValue = this.currencyPipe.transform(value, 'INR', 'symbol', '1.0-2', 'en-IN');
        // const formatValue = Number(value).toLocaleString('en-IN');
        const fractionDigit = this.numberFormatColorFraction ? 2 : 0;
        const formatValue = Number(value).toLocaleString('en-IN', 
                                { style: 'decimal', minimumIntegerDigits: 1, 
                                    minimumFractionDigits: fractionDigit, maximumFractionDigits: fractionDigit });

        if (value < 0) {
            const styles = this.numberFormatColorBg ? negativeNumberFormatColorBgStyles : negativeNumberFormatColorStyles;
            Object.keys(styles).forEach(newStyle => {
                this.renderer.setStyle(
                    this.element, `${newStyle}`, styles[newStyle]
                );
            });
        }

        this.renderer.setProperty(
            this.element, `innerText`, formatValue
        );
    }
}