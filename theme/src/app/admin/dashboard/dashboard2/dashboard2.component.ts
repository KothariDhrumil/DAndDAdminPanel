import { Component, OnInit } from '@angular/core';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexStroke,
  ApexMarkers,
  ApexYAxis,
  ApexGrid,
  ApexTitleSubtitle,
  ApexTooltip,
  ApexLegend,
  ApexFill,
  ApexResponsive,
  ApexNonAxisChartSeries,
  NgApexchartsModule,
} from 'ng-apexcharts';
import { NgScrollbar } from 'ngx-scrollbar';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { InfoBox1Component } from '@shared/components/info-box1/info-box1.component';
import { MatCardModule } from '@angular/material/card';
import { RecentCommentsComponent } from '@shared/components/recent-comments/recent-comments.component';
import {
  ProgressTableComponent,
  SubjectProgress,
} from '@shared/components/progress-table/progress-table.component';
import { EarningSourceComponent } from '@shared/components/earning-source/earning-source.component';
import { TableCardComponent } from '@shared/components/table-card/table-card.component';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  series2: ApexNonAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  markers: ApexMarkers;
  colors: string[];
  yaxis: ApexYAxis;
  grid: ApexGrid;
  legend: ApexLegend;
  tooltip: ApexTooltip;
  fill: ApexFill;
  title: ApexTitleSubtitle;
  responsive: ApexResponsive[];
  labels: string[];
};

@Component({
    selector: 'app-dashboard2',
    templateUrl: './dashboard2.component.html',
    styleUrls: ['./dashboard2.component.scss'],
    imports: [
        BreadcrumbComponent,
        MatProgressBarModule,
        NgApexchartsModule,
        MatCardModule,
        MatButtonModule,
        MatMenuModule,
        MatIconModule,
        NgScrollbar,
        InfoBox1Component,
        RecentCommentsComponent,
        ProgressTableComponent,
        EarningSourceComponent,
        TableCardComponent,
    ]
})
export class Dashboard2Component implements OnInit {
  public lineChartOptions!: Partial<ChartOptions>;
  public pieChartOptions!: Partial<ChartOptions>;
  //  color: ["#3FA7DC", "#F6A025", "#9BC311"],
  constructor() {
    // controller code
  }
  ngOnInit() {
    this.chart1();
    this.chart2();
  }

  private chart1() {
    this.lineChartOptions = {
      series: [
        {
          name: 'Employee 1',
          data: [70, 200, 80, 180, 170, 105, 210],
        },
        {
          name: 'Employee 2',
          data: [80, 250, 30, 120, 260, 100, 180],
        },
        {
          name: 'Employee 3',
          data: [85, 130, 85, 225, 80, 190, 120],
        },
      ],
      chart: {
        height: 350,
        type: 'line',
        foreColor: '#9aa0ac',
        dropShadow: {
          enabled: true,
          color: '#000',
          top: 18,
          left: 7,
          blur: 10,
          opacity: 0.2,
        },
        toolbar: {
          show: false,
        },
      },
      colors: ['#A5A5A5', '#875692', '#4CB5AC'],
      stroke: {
        curve: 'smooth',
      },
      grid: {
        show: true,
        borderColor: '#9aa0ac',
        strokeDashArray: 1,
      },
      markers: {
        size: 3,
      },
      xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        title: {
          text: 'Month',
        },
      },
      yaxis: {
        // opposite: true,
        title: {
          text: 'Clients',
        },
      },
      legend: {
        position: 'top',
        horizontalAlign: 'right',
        floating: true,
        offsetY: -25,
        offsetX: -5,
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

  private chart2() {
    this.pieChartOptions = {
      series2: [44, 55, 13, 43, 22],
      chart: {
        type: 'donut',
        width: 210,
      },
      legend: {
        show: false,
      },
      dataLabels: {
        enabled: false,
      },
      labels: ['India', 'USA', 'Shrilanka', 'Australia', 'Japan'],
      responsive: [
        {
          breakpoint: 480,
          options: {},
        },
      ],
    };
  }

  //recent comments

  comments = [
    {
      name: 'Airi Satou',
      message: 'Lorem ipsum dolor sit amet, id quo eruditi eloquentiam.',
      timestamp: '7 hours ago',
      imgSrc: 'assets/images/user/user6.jpg',
      colorClass: 'col-green',
    },
    {
      name: 'Sarah Smith',
      message: 'Lorem ipsum dolor sit amet, id quo eruditi eloquentiam.',
      timestamp: '1 hour ago',
      imgSrc: 'assets/images/user/user4.jpg',
      colorClass: 'color-primary col-indigo',
    },
    {
      name: 'Cara Stevens',
      message: 'Lorem ipsum dolor sit amet, id quo eruditi eloquentiam.',
      timestamp: 'Yesterday',
      imgSrc: 'assets/images/user/user3.jpg',
      colorClass: 'color-danger col-cyan',
    },
    {
      name: 'Ashton Cox',
      message: 'Lorem ipsum dolor sit amet, id quo eruditi eloquentiam.',
      timestamp: 'Yesterday',
      imgSrc: 'assets/images/user/user7.jpg',
      colorClass: 'color-info col-orange',
      noBorder: true,
    },
    {
      name: 'Mark Hay',
      message: 'Lorem ipsum dolor sit amet, id quo eruditi eloquentiam.',
      timestamp: '1 hour ago',
      imgSrc: 'assets/images/user/user9.jpg',
      colorClass: 'color-primary col-red',
    },
  ];

  // Progress table data

  subjects: SubjectProgress[] = [
    { subject: 'Project A', progress: 30, duration: '2 Months' },
    { subject: 'Project B', progress: 55, duration: '3 Months' },
    { subject: 'Project C', progress: 67, duration: '1 Month' },
    { subject: 'Project D', progress: 70, duration: '2 Months' },
    { subject: 'Project E', progress: 24, duration: '3 Months' },
    { subject: 'Project F', progress: 77, duration: '4 Months' },
    { subject: 'Project G', progress: 41, duration: '2 Months' },
  ];

  // earning source

  sources = [
    {
      label: 'envato.com',
      percentage: 17,
      class: 'bg-green',
      labelClass: 'bg-green text-white',
    },
    {
      label: 'google.com',
      percentage: 27,
      class: 'bg-red',
      labelClass: 'bg-red text-white',
    },
    {
      label: 'yahoo.com',
      percentage: 25,
      class: 'bg-indigo',
      labelClass: 'bg-indigo text-white',
    },
    {
      label: 'store',
      percentage: 18,
      class: 'bg-orange',
      labelClass: 'bg-orange text-white',
    },
    {
      label: 'Others',
      percentage: 13,
      class: 'bg-dark',
      labelClass: 'bg-dark text-white',
    },
  ];

  // leave request data

  leaveRequestData = [
    {
      id: 'ID7865',
      name: 'Jens Brincker',
      leaveType: 'Sick Leave',
      leaveFrom: '05/22/2021',
      leaveTo: '05/27/2021',
      days: 6,
      status: 'Approve',
      img: 'assets/images/user/user1.jpg',
      detailsLink: '#/admin/leave/details',
    },
    {
      id: 'ID9357',
      name: 'Mark Harry',
      leaveType: 'Casual Leave',
      leaveFrom: '06/12/2021',
      leaveTo: '06/15/2021',
      days: 4,
      status: 'Reject',
      img: 'assets/images/user/user2.jpg',
      detailsLink: '#/admin/leave/details',
    },
    {
      id: 'ID3987',
      name: 'Anthony Davie',
      leaveType: 'Marriage Leave',
      leaveFrom: '02/02/2021',
      leaveTo: '02/12/2021',
      days: 6,
      status: 'Pending',
      img: 'assets/images/user/user3.jpg',
      detailsLink: '#/admin/leave/details',
    },
    {
      id: 'ID2483',
      name: 'David Perry',
      leaveType: 'Maternity leave',
      leaveFrom: '01/10/2021',
      leaveTo: '03/10/2021',
      days: 90,
      status: 'Approve',
      img: 'assets/images/user/user4.jpg',
      detailsLink: '#/admin/leave/details',
    },
    {
      id: 'ID2986',
      name: 'John Doe',
      leaveType: 'Unpaid Leave',
      leaveFrom: '05/20/2021',
      leaveTo: '05/22/2021',
      days: 3,
      status: 'Reject',
      img: 'assets/images/user/user5.jpg',
      detailsLink: '#/admin/leave/details',
    },
    {
      id: 'ID1267',
      name: 'Sarah Smith',
      leaveType: 'Sick Leave',
      leaveFrom: '07/10/2021',
      leaveTo: '07/11/2021',
      days: 2,
      status: 'Approve',
      img: 'assets/images/user/user6.jpg',
      detailsLink: '#/admin/leave/details',
    },
    {
      id: 'ID3398',
      name: 'Cara Stevens',
      leaveType: 'Casual leave',
      leaveFrom: '04/11/2021',
      leaveTo: '04/13/2021',
      days: 3,
      status: 'Pending',
      img: 'assets/images/user/user7.jpg',
      detailsLink: '#/admin/leave/details',
    },
    {
      id: 'ID9965',
      name: 'Ashton Cox',
      leaveType: 'Sick Leave',
      leaveFrom: '05/14/2021',
      leaveTo: '05/15/2021',
      days: 2,
      status: 'Approve',
      img: 'assets/images/user/user8.jpg',
      detailsLink: '#/admin/leave/details',
    },
  ];

  leaveRequestColumn = [
    { def: 'id', label: 'ID', type: 'text' },
    { def: 'name', label: 'Employee Name', type: 'text' },
    { def: 'leaveType', label: 'Leave Type', type: 'text' },
    { def: 'leaveFrom', label: 'Leave From', type: 'date' },
    { def: 'leaveTo', label: 'Leave To', type: 'date' },
    { def: 'days', label: 'Days', type: 'number' },
    { def: 'status', label: 'Status', type: 'badge' },
    { def: 'detailsLink', label: 'Details', type: 'button' },
  ];
}
