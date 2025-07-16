import { Component, OnInit, ViewChild } from '@angular/core';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTooltip,
  ApexYAxis,
  ApexPlotOptions,
  ApexStroke,
  ApexLegend,
  ApexNonAxisChartSeries,
  ApexMarkers,
  ApexGrid,
  ApexTitleSubtitle,
  NgApexchartsModule,
} from 'ng-apexcharts';
import { RouterLink } from '@angular/router';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { StatisticCard2Component } from '@shared/components/statistic-card2/statistic-card2.component';
import { MatCardModule } from '@angular/material/card';
import { ProjectHoursComponent } from '@shared/components/project-hours/project-hours.component';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { TableCardComponent } from '@shared/components/table-card/table-card.component';
import { ReviewWidgetComponent } from '@shared/components/review-widget/review-widget.component';
export type areaChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  dataLabels: ApexDataLabels;
  legend: ApexLegend;
  colors: string[];
};

export type restRateChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  markers: ApexMarkers;
  colors: string[];
  yaxis: ApexYAxis;
  grid: ApexGrid;
  tooltip: ApexTooltip;
  legend: ApexLegend;
  title: ApexTitleSubtitle;
};
export type performanceRateChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  markers: ApexMarkers;
  colors: string[];
  yaxis: ApexYAxis;
  grid: ApexGrid;
  tooltip: ApexTooltip;
  legend: ApexLegend;
  title: ApexTitleSubtitle;
};

export type radialChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  labels: string[];
  colors: string[];
  plotOptions: ApexPlotOptions;
};
@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
    imports: [
        BreadcrumbComponent,
        NgApexchartsModule,
        MatCardModule,
        NgScrollbarModule,
        StatisticCard2Component,
        ProjectHoursComponent,
        TableCardComponent,
        ReviewWidgetComponent,
    ]
})
export class DashboardComponent implements OnInit {
  @ViewChild('chart') chart!: ChartComponent;
  public areaChartOptions!: Partial<areaChartOptions>;
  public radialChartOptions!: Partial<radialChartOptions>;
  public restRateChartOptions!: Partial<restRateChartOptions>;
  public performanceRateChartOptions!: Partial<performanceRateChartOptions>;

  constructor() {
    // code here
  }
  ngOnInit() {
    this.chart1();
    this.chart2();
    this.chart3();
    this.chart4();
  }
  private chart1() {
    this.areaChartOptions = {
      series: [
        {
          name: 'New Clients',
          data: [31, 40, 28, 51, 42, 85, 77],
        },
        {
          name: 'Old Clients',
          data: [11, 32, 45, 32, 34, 52, 41],
        },
      ],
      chart: {
        height: 350,
        type: 'area',
        toolbar: {
          show: false,
        },
        foreColor: '#9aa0ac',
      },
      colors: ['#7D4988', '#66BB6A'],
      dataLabels: {
        enabled: false,
      },
      stroke: {
        curve: 'smooth',
      },
      xaxis: {
        type: 'datetime',
        categories: [
          '2018-09-19T00:00:00.000Z',
          '2018-09-19T01:30:00.000Z',
          '2018-09-19T02:30:00.000Z',
          '2018-09-19T03:30:00.000Z',
          '2018-09-19T04:30:00.000Z',
          '2018-09-19T05:30:00.000Z',
          '2018-09-19T06:30:00.000Z',
        ],
      },
      legend: {
        show: true,
        position: 'top',
        horizontalAlign: 'center',
        offsetX: 0,
        offsetY: 0,
      },

      tooltip: {
        x: {
          format: 'dd/MM/yy HH:mm',
        },
      },
    };
  }
  private chart2() {
    this.radialChartOptions = {
      series: [44, 55, 67],
      chart: {
        height: 265,
        type: 'radialBar',
      },
      plotOptions: {
        radialBar: {
          dataLabels: {
            name: {
              fontSize: '22px',
            },
            value: {
              fontSize: '16px',
            },
            total: {
              show: true,
              label: 'Total',
              formatter: function () {
                return '249';
              },
            },
          },
        },
      },
      colors: ['#ffc107', '#3f51b5', '#8bc34a'],

      labels: ['Face TO Face', 'E-Consult', 'Available'],
    };
  }

  private chart3() {
    this.restRateChartOptions = {
      series: [
        {
          name: 'Hours',
          data: [69, 75, 72, 69, 75, 66, 80],
        },
      ],
      chart: {
        height: 375,
        type: 'line',
        dropShadow: {
          enabled: true,
          color: '#000',
          top: 18,
          left: 7,
          blur: 10,
          opacity: 0.2,
        },
        foreColor: '#9aa0ac',
        toolbar: {
          show: false,
        },
      },
      colors: ['#6777EF'],
      dataLabels: {
        enabled: true,
      },
      stroke: {
        curve: 'smooth',
      },
      markers: {
        size: 1,
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        title: {
          text: 'Months',
        },
      },
      yaxis: {
        title: {
          text: 'Hours',
        },
      },
      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }
  private chart4() {
    this.performanceRateChartOptions = {
      series: [
        {
          name: 'Hours',
          data: [113, 120, 130, 120, 125, 119, 126],
        },
      ],
      chart: {
        height: 375,
        type: 'line',
        dropShadow: {
          enabled: true,
          color: '#000',
          top: 18,
          left: 7,
          blur: 10,
          opacity: 0.2,
        },
        foreColor: '#9aa0ac',
        toolbar: {
          show: false,
        },
      },
      colors: ['#976DA0'],
      dataLabels: {
        enabled: true,
      },
      stroke: {
        curve: 'smooth',
      },
      markers: {
        size: 1,
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        title: {
          text: 'Months',
        },
      },
      yaxis: {
        title: {
          text: 'Hours',
        },
      },
      tooltip: {
        theme: 'dark',
        marker: {
          show: true,
        },
        x: {
          show: true,
        },
      },
    };
  }

  // invoice data

  invoiceData = [
    {
      invoiceNo: '#IN7865',
      name: 'John Doe',
      dueDate: '12/05/2016',
      status: 'Paid',
      total: '$500',
      img: 'assets/images/user/user1.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN2301',
      name: 'Sarah Smith',
      dueDate: '03/31/2016',
      status: 'Not Paid',
      total: '$372',
      img: 'assets/images/user/user2.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN7239',
      name: 'Airi Satou',
      dueDate: '04/14/2017',
      status: 'Partially Paid',
      total: '$1038',
      img: 'assets/images/user/user3.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN1482',
      name: 'Angelica Ramos',
      dueDate: '08/11/2017',
      status: 'Paid',
      total: '$872',
      img: 'assets/images/user/user4.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN8526',
      name: 'Ashton Cox',
      dueDate: '02/15/2018',
      status: 'Not Paid',
      total: '$2398',
      img: 'assets/images/user/user5.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN2473',
      name: 'Cara Stevens',
      dueDate: '01/28/2017',
      status: 'Paid',
      total: '$834',
      img: 'assets/images/user/user6.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN7366',
      name: 'Jacob Ryan',
      dueDate: '03/11/2017',
      status: 'Partially Paid',
      total: '$147',
      img: 'assets/images/user/user7.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN5642',
      name: 'Emily Walker',
      dueDate: '09/12/2018',
      status: 'Paid',
      total: '$650',
      img: 'assets/images/user/user8.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN1457',
      name: 'Michael Brown',
      dueDate: '04/20/2019',
      status: 'Not Paid',
      total: '$1220',
      img: 'assets/images/user/user9.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN9083',
      name: 'Olivia Green',
      dueDate: '10/03/2020',
      status: 'Partially Paid',
      total: '$850',
      img: 'assets/images/user/user10.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN3379',
      name: 'David Lee',
      dueDate: '06/25/2021',
      status: 'Paid',
      total: '$1295',
      img: 'assets/images/user/user11.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
    {
      invoiceNo: '#IN9874',
      name: 'Sophia Johnson',
      dueDate: '01/18/2022',
      status: 'Not Paid',
      total: '$320',
      img: 'assets/images/user/user2.jpg',
      invoiceLink: '#/admin/accounts/invoice',
    },
  ];

  invoiceColumnDefinitions = [
    { def: 'invoiceNo', label: 'Invoice No', type: 'text' },
    { def: 'name', label: 'Generated By', type: 'text' },
    { def: 'dueDate', label: 'Due Date', type: 'date' },
    { def: 'status', label: 'Status', type: 'badge' },
    { def: 'total', label: 'Total', type: 'currency' },
    { def: 'invoiceLink', label: 'Download', type: 'file' },
  ];

  // review list

  reviewList = [
    {
      name: 'Alis Smith',
      timeAgo: 'a week ago',
      rating: 3.5,
      comment:
        'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam vel rutrum ex, at ornare mi. In quis scelerisque dui, eget rhoncus orci. Fusce et sodales ipsum. Nam id nunc euismod, aliquet arcu quis, mattis nisi.',
      imageUrl: 'assets/images/user/user1.jpg',
    },
    {
      name: 'John Dio',
      timeAgo: 'a week ago',
      rating: 2.5,
      comment:
        'Nam quis ligula est. Nunc sed risus non turpis tristique tempor. Ut sollicitudin faucibus magna nec gravida. Suspendisse ullamcorper justo vel porta imperdiet. Nunc nec ipsum vel augue placerat faucibus.',
      imageUrl: 'assets/images/user/user2.jpg',
    },
  ];
}
